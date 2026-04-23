using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Questions.Queries.GetQuestionDetails
{
    public class GetQuestionDetailsHandler : IRequestHandler<GetQuestionDetailsQuery, Response<QuestionDetailsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public GetQuestionDetailsHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }

        public async Task<Response<QuestionDetailsResponseDto>> Handle(GetQuestionDetailsQuery request, CancellationToken cancellationToken)
        {
            var question = await _unitOfWork.Questions.GetDetailsAsync(request.Id, false, cancellationToken);

            if (question == null)
                return _responseHandler.NotFound<QuestionDetailsResponseDto>(SystemMessages.RECORD_NOT_FOUND);

            var dto = _mapper.Map<QuestionDetailsResponseDto>(question);
            
            // Add statistics or derived fields if not handled by mapper
            dto.AuthorName = question.User?.FullName ?? "Unknown";
            dto.AuthorProfileImageUrl = question.User?.ProfileImageUrl;
            dto.AuthorReputation = question.User?.Reputation ?? 0;
            dto.CategoryName = question.Category?.Name ?? "Uncategorized";

            return _responseHandler.Success(dto, SystemMessages.DATA_RETRIEVED);
        }
    }
}
