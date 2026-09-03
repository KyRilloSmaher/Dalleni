



using AutoMapper;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Application.Features.Services.Queries.GetServicesByCategory;
using Dalleni.Application.Features.Services.Queries.SearchServices;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServicesByOfficialEntity
{
    internal sealed class GetServicesByOfficialEntityHandler: IRequestHandler<GetServicesByOfficialEntityQuery, Response<IEnumerable<ServiceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper ;
        public GetServicesByOfficialEntityHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ServiceDto>>> Handle(GetServicesByOfficialEntityQuery request,CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.Services.GetByOfficialEntityIdAsync(request.OfficialEntityId);

            if (service is null || !service.Any())
            {
                return _responseHandler.NotFound<IEnumerable<ServiceDto>>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var dto = _mapper.Map<IEnumerable<ServiceDto>>(service);

            return _responseHandler.Success(dto,SystemMessages.SUCCESS);
        }
    }
}