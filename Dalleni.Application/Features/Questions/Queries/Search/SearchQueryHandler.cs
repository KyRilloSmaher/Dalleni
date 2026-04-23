using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.Search
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, Response<PaginatedResult<QuestionSummaryDto>>>
    {
        private readonly ISearchService _searchService;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public SearchQueryHandler(
            ISearchService searchService,
            IResponseHandler responseHandler,
            IMapper mapper)
        {
            _searchService = searchService;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(
            SearchQuery request,
            CancellationToken cancellationToken)
        {
            var paged = request.pagedRequest;

            var searchResults = await _searchService.HybridSearchAsync(
                request.query,
                /*request.Category*/ null,
                /*request.Tags*/null,
                paged.PageNumber,
                paged.PageSize
            );
            var mapped = _mapper.Map<List<QuestionSummaryDto>>(searchResults);

            // Since Azure already paginates, total count is tricky in Free tier
            var result = PaginatedResult<QuestionSummaryDto>.Success(
                mapped,
                mapped.Count,
                paged.PageNumber,
                paged.PageSize
            );

            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}