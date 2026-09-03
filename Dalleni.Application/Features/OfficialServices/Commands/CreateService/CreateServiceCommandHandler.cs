using AutoMapper;
using Dalleni.Application.DTOs.Requests.Services;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Commands.CreateService
{
    internal sealed class CreateServiceHandler: IRequestHandler<CreateServiceCommand, Response<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public CreateServiceHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<ServiceDto>> Handle(CreateServiceCommand request,CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var officialEntity =await _unitOfWork.OfficialEntities.GetByIdAsync(dto.OfficialEntityId,false);

            if (officialEntity is null || officialEntity.IsDeleted)
            {
                return _responseHandler.NotFound<ServiceDto>(SystemMessages.OFFICIAL_ENTITY_NOT_FOUND);
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

            var service = _mapper.Map<Service>(dto);
            await _unitOfWork.Services.AddAsync(service,cancellationToken);
         

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            var responseServiceDto = _mapper.Map<ServiceDto>(service); 
            return _responseHandler.Success(responseServiceDto,SystemMessages.SUCCESS);
        }
    }
}