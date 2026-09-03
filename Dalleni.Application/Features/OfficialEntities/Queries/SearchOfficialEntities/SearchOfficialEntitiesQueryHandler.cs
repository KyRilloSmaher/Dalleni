
using AutoMapper;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.OfficialEntities.Queries.SearchOfficialEntities
{

    internal sealed class SearchOfficialEntitiesQueryHandler: IRequestHandler<SearchOfficialEntitiesQuery, Response<IEnumerable<OfficialEntityDto>>>
    {
        private readonly IResponseHandler _responseHandler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchOfficialEntitiesQueryHandler(IResponseHandler responseHandler, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _responseHandler = responseHandler;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task< Response<IEnumerable<OfficialEntityDto>>> Handle(SearchOfficialEntitiesQuery request,CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Keyword))
            {
                return _responseHandler.BadRequest<
                    IEnumerable<OfficialEntityDto>>(
                    "Search keyword is required.");
            }

            var entities = await _unitOfWork.OfficialEntities.SearchAsync(
                            request.Keyword.Trim(),
                            cancellationToken);
            var result =  _mapper.Map<IEnumerable<OfficialEntityDto>>(entities);
            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
   
}