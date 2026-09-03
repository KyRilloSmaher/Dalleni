
using AutoMapper;
using Dalleni.Application.Features.Services.Commands.DeleteService;
using Dalleni.Application.Features.Services.Commands.RestoreService;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.CreateService
{
    internal sealed class RestoreServiceHandler: IRequestHandler<RestoreServiceCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public RestoreServiceHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(RestoreServiceCommand request,CancellationToken cancellationToken)
        {
            var service =await _unitOfWork.Services.GetByIdAsync(request.Id,true);

            if (service is null || service.IsDeleted)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var membership =await _unitOfWork.OfficialEntityMemberships
                                                .GetByUserAsync(
                                                    request.UserId,
                                                    cancellationToken);

            if (membership is null || !membership.IsActive)
            {
                return _responseHandler.Unauthorized<bool>("You are not a member of this official entity.");
            }

            if (membership.CanManageServices())
            {
                return _responseHandler.Unauthorized<bool>("You do not have permission to Restore services.");
            }
            service.Restore();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success(true,SystemMessages.SUCCESS);
        }
    }
}