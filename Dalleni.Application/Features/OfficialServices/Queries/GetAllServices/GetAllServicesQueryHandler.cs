


using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Services.Queries.GetAllServices
{
    internal sealed class SearchServicesHandler: IRequestHandler<GetAllServicesQuery,Response<PaginatedResult<ServiceDto>>>
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

        public async Task<Response<PaginatedResult<ServiceDto>>> Handle(GetAllServicesQuery request,CancellationToken cancellationToken)
        {

            var PagedRequest = request.request;
            var query = await _unitOfWork.Services.GetAllAsQueryableAsync();
            var projected = _mapper.ProjectTo<ServiceDto>(query);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber , PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}