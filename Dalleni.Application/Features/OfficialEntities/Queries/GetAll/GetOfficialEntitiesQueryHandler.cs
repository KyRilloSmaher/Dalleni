using AutoMapper;
using Dalleni.Application.Commans.Extensions;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.GetAllOfficialEntities
{
    internal sealed class GetAllOfficialEntitiesQueryHandler: IRequestHandler<GetAllOfficialEntitiesQuery, Response<PaginatedResult<OfficialEntityDto>>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOfficialEntitiesQueryHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task< Response<PaginatedResult<OfficialEntityDto>>> Handle(GetAllOfficialEntitiesQuery request,CancellationToken cancellationToken)
        {
            var PagedRequest = request.request;
            var query = await _unitOfWork.OfficialEntities.GetAllAsQueryableAsync();
            var projected = _mapper.ProjectTo<OfficialEntityDto>(query);
            var result = await projected.ToPaginatedListAsync(PagedRequest.PageNumber , PagedRequest.PageSize);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}