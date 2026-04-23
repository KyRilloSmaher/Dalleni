using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Commands.CloseQuestion
{
    public class CloseQuestionHandler : IRequestHandler<CloseQuestionCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<CloseQuestionHandler> _logger;

        public CloseQuestionHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<CloseQuestionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(CloseQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.Id, true);
                if (question == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                if (question.UserId != request.UserId)
                    return _responseHandler.Unauthorized<bool>(SystemMessages.ACCESS_DENIED);

                question.Close();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing question {QuestionId}", request.Id);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
