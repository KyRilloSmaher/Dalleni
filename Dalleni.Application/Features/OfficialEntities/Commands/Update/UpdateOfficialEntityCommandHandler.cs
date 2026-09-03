
using AutoMapper;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.OfficialEntities.Commands.UpdateOfficialEntity
{
    internal sealed class UpdateOfficialEntityCommandHandler
        : IRequestHandler<UpdateOfficialEntityCommand, Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageUploaderService _imageUploaderService;
        private readonly ILogger<UpdateOfficialEntityCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateOfficialEntityCommandHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IImageUploaderService imageUploaderService, ILogger<UpdateOfficialEntityCommandHandler> logger, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _imageUploaderService = imageUploaderService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(UpdateOfficialEntityCommand request,CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.OfficialEntities.GetByIdAsync(request.Id, true);

            if (entity is null)
            {
                return _responseHandler.NotFound<bool>( SystemMessages.OFFICIAL_ENTITY_NOT_FOUND);
            }

            var membership = await _unitOfWork.OfficialEntityMemberships
                                                .GetByUserAndEntityAsync(
                                                    request.userId,
                                                    entity.Id,
                                                    cancellationToken);

            if (membership is null || !membership.IsActive)
            {
                return _responseHandler.Unauthorized<bool>(SystemMessages.NOT_OFFICIAL_ENTITY_MEMBER);
            }

            if (membership.Role != EntityRole.Owner && membership.Role != EntityRole.Admin)
            {
                return _responseHandler.Unauthorized<bool>(SystemMessages.NO_PERMISSION_TO_UPDATE_OFFICIAL_ENTITY);
            }

            string? oldLogoUrl = entity.LogoUrl;
            string? newLogoUrl = null;

            try
            {
                /*
                 * Upload the new logo first.
                 * We do NOT delete the old logo yet.
                 */
                if (request.Dto.Logo is not null)
                {
                    var uploadResult = await _imageUploaderService.UploadImageAsync(request.Dto.Logo,ImageFolder.logos);

                    if (uploadResult.Error is not null)
                    {
                        return _responseHandler.BadRequest<bool>(SystemMessages.UPLOAD_IMAGE_FAILED);
                    }

                    newLogoUrl = uploadResult.Url.ToString();
                }

                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                _mapper.Map(request.Dto, entity);

                if (newLogoUrl is not null)
                {
                    entity.LogoUrl = newLogoUrl;
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                /*
                 * Delete the old logo only after the database
                 * transaction has successfully committed.
                 */
                if (newLogoUrl is not null && !string.IsNullOrWhiteSpace(oldLogoUrl))
                {
                    try
                    {
                        await _imageUploaderService.DeleteImageAsync(oldLogoUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(
                            ex,
                            "Official entity {OfficialEntityId} was updated successfully, " +
                            "but the old logo could not be deleted: {OldLogoUrl}",
                            entity.Id,
                            oldLogoUrl);
                    }
                }

                return _responseHandler.Success(true, SystemMessages.OFFICIAL_ENTITY_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to update official entity {OfficialEntityId}",
                    request.Id);

                await _unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                /*
                 * The database update failed after the new logo
                 * was uploaded, so remove the new logo to prevent
                 * an orphaned file.
                 */
                if (newLogoUrl is not null)
                {
                    try
                    {
                        await _imageUploaderService.DeleteImageAsync(
                            newLogoUrl);
                    }
                    catch (Exception cleanupException)
                    {
                        _logger.LogError(
                            cleanupException,
                            "Failed to cleanup newly uploaded logo " +
                            "{NewLogoUrl} after update failure",
                            newLogoUrl);
                    }
                }

                throw;
            }
        }
    }
}
