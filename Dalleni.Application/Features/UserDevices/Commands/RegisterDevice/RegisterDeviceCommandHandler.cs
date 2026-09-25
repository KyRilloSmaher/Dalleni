using Dalleni.Domin.Models;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Helpers;

namespace Dalleni.Application.Features.UserDevices.Commands.RegisterDevice
{
    public class RegisterDeviceCommandHandler : IRequestHandler<RegisterDeviceCommand, Response<Guid>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;


        public RegisterDeviceCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }
       

        public async Task<Response<Guid>> Handle( RegisterDeviceCommand request,CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var existingDevice =await _unitOfWork.UserDevices.GetByDeviceTokenAsync(
                    request.Request.DeviceToken,
                    asTracked: true,
                    cancellationToken);

            if (existingDevice is not null)
            {
                if (existingDevice.UserId != userId)
                {
                    return _responseHandler.BadRequest<Guid>(SystemMessages.DEVICE_ALREADY_REGISTERED);
                }

                existingDevice.UpdateToken(request.Request.DeviceToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success<Guid>(existingDevice.Id);
            }

            var device = UserDevice.Create(
                userId,
                request.Request.DeviceToken,
                request.Request.Platform);

            await _unitOfWork.UserDevices.AddAsync(device,cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success<Guid>(device. Id, SystemMessages.RECORD_ADDED);
        }
    }
}