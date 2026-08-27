using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Application.Features.Questions.Queries.GetSimilars;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Dalleni.Application.Features.Questions.Queries.GetSimilars
{
    public class SimilarQuestionsQueryHandler : IRequestHandler<SimilarQuestionsQuery, Response<IEnumerable<QuestionDetailsResponseDto>>>
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

        public async Task<Response<IEnumerable<QuestionDetailsResponseDto>>> Handle(
            SimilarQuestionsQuery request, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Question))
            {
                return _responseHandler.BadRequest<IEnumerable<QuestionDetailsResponseDto>>(SystemMessages.BAD_REQUEST);
            }

            try
            {

                var searchResults = await _searchService.HybridSearchAsync(
                    query: request.Question,      // The question text
                    category: null,                // Optional: filter by category
                    tags: null,                    // Optional: filter by tags
                    pageNumber: 1,
                    pageSize: 10                   // Get top 10 similar questions
                );

                if (searchResults == null || !searchResults.Any())
                {
                    return _responseHandler.Success<IEnumerable<QuestionDetailsResponseDto>>(
                        new List<QuestionDetailsResponseDto>(), 
                        SystemMessages.NO_DATA_FOUND);
                }

                var similarQuestions = _mapper.Map<IEnumerable<QuestionDetailsResponseDto>>(searchResults);

                return _responseHandler.Success(similarQuestions, SystemMessages.DATA_RETRIEVED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to find similar questions for: {Question}", request.Question);
                return _responseHandler.ServerError<IEnumerable<QuestionDetailsResponseDto>>("Failed to find similar questions");
            }
        }
    }
}