using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Users.Commands.AdjustReputation
{
    public class AdjustReputationHandler : IRequestHandler<AdjustReputationCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<AdjustReputationHandler> _logger;

        public AdjustReputationHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<AdjustReputationHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(AdjustReputationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserManager.FindByIdAsync(request.UserId, true);
                if (user == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.USER_NOT_FOUND);

                user.AdjustReputation(request.Delta);

                await _unitOfWork.UserManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adjusting reputation for user {UserId}", request.UserId);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
