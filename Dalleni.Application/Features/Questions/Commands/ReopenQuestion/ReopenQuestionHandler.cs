using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Commands.ReopenQuestion
{
    public class ReopenQuestionHandler : IRequestHandler<ReopenQuestionCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<ReopenQuestionHandler> _logger;

        public ReopenQuestionHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<ReopenQuestionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(ReopenQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.Id, true);
                if (question == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                if (question.UserId != request.UserId)
                    return _responseHandler.Unauthorized<bool>(SystemMessages.ACCESS_DENIED);

                question.Reopen();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reopening question {QuestionId}", request.Id);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
