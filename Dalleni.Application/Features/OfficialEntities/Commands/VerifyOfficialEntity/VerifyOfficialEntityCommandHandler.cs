using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Commands.VerifyOfficialEntity
{
    internal sealed class VerifyOfficialEntityCommandHandler
        : IRequestHandler<
            VerifyOfficialEntityCommand,
            Response<bool>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyOfficialEntityCommandHandler(
            IResponseHandler responseHandler,
            IUnitOfWork unitOfWork)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(
            VerifyOfficialEntityCommand request,
            CancellationToken cancellationToken)
        {
            var entity =await _unitOfWork.OfficialEntities.GetByIdAsync(request.Id,true);

            if (entity is null)
            {
                return _responseHandler.NotFound<bool>(
                    "Official entity was not found.");
            }

            if (entity.IsVerified)
            {
                return _responseHandler.BadRequest<bool>("Official entity is already verified.");
            }

            entity.Verify();

            return _responseHandler.Success(
                true,
                "Official entity verified successfully.");
        }
    }
}