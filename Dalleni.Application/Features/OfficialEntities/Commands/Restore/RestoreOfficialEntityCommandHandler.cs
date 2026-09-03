using Dalleni.Domin.Enums;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.RestoreOfficialEntity
{
    internal sealed class RestoreOfficialEntityCommandHandler: IRequestHandler<RestoreOfficialEntityCommand,Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;

        public RestoreOfficialEntityCommandHandler(IResponseHandler responseHandler,IUnitOfWork unitOfWork)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(RestoreOfficialEntityCommand request,CancellationToken cancellationToken)
        {
            var entity =await _unitOfWork.OfficialEntities.GetByIdAsync(request.Id);

            if (entity is null)
            {
                return _responseHandler.NotFound<bool>( "Official entity was not found.");
            }

            var membership =await _unitOfWork.OfficialEntityMemberships.GetByUserAndEntityAsync(
                        request.userId,
                        entity.Id,
                        cancellationToken);

            if (membership is null ||!membership.IsActive)
            {
                return _responseHandler.Unauthorized<bool>("You are not a member of this official entity.");
            }

            if (membership.Role != EntityRole.Owner)
            {
                return _responseHandler.Unauthorized<bool>("Only the owner can delete the official entity.");
            }
            entity.Restore();

            return _responseHandler.Success(
                true,
                "Official entity restored successfully.");
        }
    }
}