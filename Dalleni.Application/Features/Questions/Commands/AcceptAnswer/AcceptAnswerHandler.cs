using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Commands.AcceptAnswer
{
    public class AcceptAnswerHandler : IRequestHandler<AcceptAnswerCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<AcceptAnswerHandler> _logger;

        public AcceptAnswerHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<AcceptAnswerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.QuestionId, true);
                if (question == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                if (question.UserId != request.UserId)
                    return _responseHandler.Unauthorized<bool>(SystemMessages.ACCESS_DENIED);

                var answer = await _unitOfWork.Answers.GetByIdAsync(request.AnswerId, true);
                if (answer == null || answer.QuestionId != request.QuestionId)
                    return _responseHandler.BadRequest<bool>("Invalid answer for this question.");

                // 1. Accept on question
                question.AcceptAnswer(request.AnswerId);

                // 2. Mark answer as accepted (Domain logic in Answer entity if exists)
                answer.Accept(); // Let's check if Answer has Accept method

                // 3. Update stats for answer author
                var answerAuthor = await _unitOfWork.UserManager.FindByIdAsync(answer.UserId, true);
                if (answerAuthor != null)
                {
                    answerAuthor.OnAnswerAccepted();
                    await _unitOfWork.UserManager.UpdateAsync(answerAuthor);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting answer {AnswerId} for question {QuestionId}", request.AnswerId, request.QuestionId);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
