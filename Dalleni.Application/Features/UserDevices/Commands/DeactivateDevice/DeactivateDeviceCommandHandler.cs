using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.UserDevices.Commands.DeactivateDevice
{
    public class DeactivateDeviceCommandHandler: IRequestHandler<DeactivateDeviceCommand, Response<bool>>
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

    
        public DeactivateDeviceCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }
       
        public async Task<Response<bool>> Handle(DeactivateDeviceCommand request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var device = await _unitOfWork.UserDevices.GetByIdAsync(
                request.DeviceId,
                asTracked: true,
                cancellationToken);

            if (device is null)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }

            if (device.UserId != userId)
            {
                return _responseHandler.Forbidden<bool>(SystemMessages.UNAUTHORIZED);
            }

            device.MarkAsInactive();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success<bool>(true);
        }
    }
}