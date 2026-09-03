using AutoMapper;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.OfficialEntities.Create
{
    internal sealed class CreateOfficialEntityCommandHandler: IRequestHandler<CreateOfficialEntityCommand, Response<Guid>>
{
    private readonly IResponseHandler _responseHandler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageUploaderService _imageUploaderService;
    private readonly ILogger<CreateOfficialEntityCommandHandler> _logger;

    public CreateOfficialEntityCommandHandler(IResponseHandler responseHandler,IUnitOfWork unitOfWork,IMapper mapper,IImageUploaderService imageUploaderService,ILogger<CreateOfficialEntityCommandHandler> logger)
    {
        _responseHandler = responseHandler;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageUploaderService = imageUploaderService;
        _logger = logger;
    }

    public async Task<Response<Guid>> Handle(CreateOfficialEntityCommand request,CancellationToken cancellationToken)
    {
        var dto = request.dto;

        var exists = await _unitOfWork.OfficialEntities.ExistsByNameAsync(dto.Name, cancellationToken);

        if (exists)
        {
            return _responseHandler.BadRequest<Guid>(SystemMessages.OFFICIAL_ENTITY_ALREADY_EXISTS);
        }

        string? uploadedImageUrl = null;

        try
        {
            if (dto.Logo is not null)
            {
                var uploadResult = await _imageUploaderService
                    .UploadImageAsync(dto.Logo, ImageFolder.logos);

                if (uploadResult.Error is not null)
                {
                    return _responseHandler.BadRequest<Guid>(
                        SystemMessages.UPLOAD_IMAGE_FAILED);
                }

                uploadedImageUrl = uploadResult.Url.ToString();
            }

            var entity = _mapper.Map<OfficialEntity>(dto);
            entity.LogoUrl = uploadedImageUrl;

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await _unitOfWork.OfficialEntities
                .AddAsync(entity, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return _responseHandler.Success(
                entity.Id,
                SystemMessages.OFFICIAL_ENTITY_CREATED);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to create official entity {EntityName}",
                dto.Name);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            throw;
        }
    }
}
}