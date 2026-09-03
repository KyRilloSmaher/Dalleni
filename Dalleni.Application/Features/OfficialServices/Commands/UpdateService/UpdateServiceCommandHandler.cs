
using AutoMapper;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Application.Features.Services.Commands.UpdateService;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.CreateService
{
    internal sealed class UpdateServiceHandler: IRequestHandler<UpdateServiceCommand, Response<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public UpdateServiceHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<ServiceDto>> Handle(UpdateServiceCommand request,CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var service =await _unitOfWork.Services.GetByIdAsync(dto.Id,true);

            if (service is null || service.IsDeleted)
            {
                return _responseHandler.NotFound<ServiceDto>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var membership =await _unitOfWork.OfficialEntityMemberships
                                                .GetByUserAndEntityAsync(
                                                    request.UserId,
                                                    dto.OfficialEntityId,
                                                    cancellationToken);

            if (membership is null || !membership.IsActive)
            {
                return _responseHandler.Unauthorized<ServiceDto>("You are not a member of this official entity.");
            }

            if (membership.CanManageServices())
            {
                return _responseHandler.Unauthorized<ServiceDto>("You do not have permission to create services.");
            }

            _mapper.Map(dto,service);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var responseServiceDto = _mapper.Map<ServiceDto>(service); 
            return _responseHandler.Success(responseServiceDto,SystemMessages.SUCCESS);
        }
    }
}