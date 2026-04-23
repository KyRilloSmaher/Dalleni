using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Answers.Commands.CreateAnswer
{
    public class CreateAnswerHandler : IRequestHandler<CreateAnswerCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<CreateAnswerHandler> _logger;

        public CreateAnswerHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<CreateAnswerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<Guid>> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.dto.QuestionId, true);
                if (question == null)
                    return _responseHandler.NotFound<Guid>(SystemMessages.RECORD_NOT_FOUND);

                if (question.IsClosed)
                    return _responseHandler.BadRequest<Guid>("Cannot answer a closed question.");

                var user = await _unitOfWork.UserManager.FindByIdAsync(request.UserId, true);
                if (user == null)
                    return _responseHandler.NotFound<Guid>(SystemMessages.USER_NOT_FOUND);

                var answer = Answer.Create(request.dto.Content, request.dto.QuestionId, request.UserId);

                await _unitOfWork.Answers.AddAsync(answer);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(answer.Id, SystemMessages.RECORD_ADDED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating answer for question {QuestionId}", request.dto.QuestionId);
                return _responseHandler.BadRequest<Guid>(ex.Message);
            }
        }
    }
}
