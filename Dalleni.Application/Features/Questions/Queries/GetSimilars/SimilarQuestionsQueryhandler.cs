using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Queries.GetSimilars
{
    public class SimilarQuestionsQueryHandler : IRequestHandler<SimilarQuestionsQuery, Response<IEnumerable<QuestionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;
        private readonly ISearchService _searchService;
        private readonly ILogger<SimilarQuestionsQueryHandler> _logger;

        public SimilarQuestionsQueryHandler(
            IUnitOfWork unitOfWork,
            IResponseHandler responseHandler,
            IMapper mapper,
            ISearchService searchService,
            ILogger<SimilarQuestionsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
            _searchService = searchService;
            _logger = logger;
        }

        public async Task<Response<IEnumerable<QuestionSummaryDto>>> Handle(
            SimilarQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Question))
            {
                return _responseHandler.BadRequest<IEnumerable<QuestionSummaryDto>>(SystemMessages.BAD_REQUEST);
            }

            try
            {
                // 1. Get search results from Azure
                var searchResults = await _searchService.HybridSearchAsync(
                    query: request.Question,
                    category: null,
                    tags: null,
                    pageNumber: 1,
                    pageSize: 10
                );

                // 2. Handle empty results
                if (searchResults == null || !searchResults.Any())
                {
                    return _responseHandler.Success<IEnumerable<QuestionSummaryDto>>(
                        new List<QuestionSummaryDto>(),
                        SystemMessages.NO_DATA_FOUND);
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
                var similarQuestions = searchResults
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

                return _responseHandler.Success(
                    similarQuestions.AsEnumerable(),
                    SystemMessages.DATA_RETRIEVED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find similar questions for: {Question}", request.Question);
                return _responseHandler.ServerError<IEnumerable<QuestionSummaryDto>>("Failed to find similar questions");
            }
        }
    }
}