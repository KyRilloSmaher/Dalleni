using AutoMapper;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServiceById
{
    internal sealed class GetServiceByIdHandler: IRequestHandler<GetServiceByIdQuery, Response<ServiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper ;
        public GetServiceByIdHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<ServiceDto>> Handle(GetServiceByIdQuery request,CancellationToken cancellationToken)
        {
            var service =await _unitOfWork.Services.GetByIdAsync(request.Id);

            if (service is null || service.IsDeleted)
            {
                return _responseHandler.NotFound<ServiceDto>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var dto = _mapper.Map<ServiceDto>(service);

            return _responseHandler.Success(dto,SystemMessages.SUCCESS);
        }
    }
}