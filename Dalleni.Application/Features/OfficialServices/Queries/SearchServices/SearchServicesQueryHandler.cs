
using AutoMapper;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Application.Features.Services.Queries.SearchServices;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetServiceById
{
    internal sealed class SearchServicesHandler: IRequestHandler<SearchServicesQuery, Response<IEnumerable<ServiceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper ;
        public SearchServicesHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<ServiceDto>>> Handle(SearchServicesQuery request,CancellationToken cancellationToken)
        {
            var service = await _unitOfWork.Services.SearchAsync(request.Keyword);

            if (service is null || !service.Any())
            {
                return _responseHandler.NotFound<IEnumerable<ServiceDto>>(SystemMessages.SERVICE_NOT_FOUND);
            }

            var dto = _mapper.Map<IEnumerable<ServiceDto>>(service);

            return _responseHandler.Success(dto,SystemMessages.SUCCESS);
        }
    }
}