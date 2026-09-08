using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.Search
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, Response<PaginatedResult<QuestionSummaryDto>>>
    {
        private readonly ISearchService _searchService;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SearchQueryHandler(
            ISearchService searchService,
            IResponseHandler responseHandler,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _searchService = searchService;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<PaginatedResult<QuestionSummaryDto>>> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var paged = request.pagedRequest;

            // 1. Get search results from Azure
            var searchResults = await _searchService.HybridSearchAsync(
                request.query,
                null,
                null,
                paged.PageNumber,
                paged.PageSize
            );

            // 2. Handle empty results
            if (!searchResults.Any())
            {
                var emptyResult = PaginatedResult<QuestionSummaryDto>.Success(
                    new List<QuestionSummaryDto>(),
                    0,
                    paged.PageNumber,
                    paged.PageSize
                );
                return _responseHandler.Success(emptyResult, SystemMessages.DATA_RETRIEVED);
            }

            // 3. Extract question IDs from search results
            var questionIds = searchResults
                .Where(r => !string.IsNullOrEmpty(r.id))
                .Select(r => Guid.Parse(r.id))
                .Distinct()
                .ToList();

            // 4. Fetch all questions with their users in ONE database call
            var questionsWithUsers = await _unitOfWork.Questions
                .GetQuestionsWithUsersByIdsAsync(questionIds, cancellationToken);

            // 5. Extract all user IDs from the fetched questions
            var userIds = questionsWithUsers.Values
                .Select(q => q.UserId)
                .Distinct()
                .ToList();

            // 6. Fetch all users in ONE database call
            var users = await _unitOfWork.Users
                .GetUsersByIdsAsync(userIds, cancellationToken);

            // 7. Map search results and enrich with user data
            var mapped = searchResults
                .Select(searchResult =>
                {
                    var dto = _mapper.Map<QuestionSummaryDto>(searchResult);
                    var questionId = Guid.Parse(searchResult.id);

                    // Enrich with user data if question exists
                    if (questionsWithUsers.TryGetValue(questionId, out var question))
                    {
                        dto.UserId = question.UserId;
                        
                        // Get user data from the users dictionary
                        if (users.TryGetValue(question.UserId, out var user))
                        {
                            dto.AuthorName = user.UserName ?? user.FullName;
                            dto.AuthorProfileImageUrl = user.ProfileImageUrl;
                            dto.AuthorReputation = user.Reputation;
                        }
                    }

                    return dto;
                })
                .ToList();

            // 8. Create paginated result
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