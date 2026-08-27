using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.Search
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, Response<PaginatedResult<QuestionDetailsResponseDto>>>
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

        public async Task<Response<PaginatedResult<QuestionDetailsResponseDto>>> Handle(
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
            var mapped = _mapper.Map<List<QuestionDetailsResponseDto>>(searchResults);

            
            var result = PaginatedResult<QuestionDetailsResponseDto>.Success(
                mapped,
                mapped.Count,
                paged.PageNumber,
                paged.PageSize
            );

            return _responseHandler.Success(result, SystemMessages.DATA_RETRIEVED);
        }
    }
}