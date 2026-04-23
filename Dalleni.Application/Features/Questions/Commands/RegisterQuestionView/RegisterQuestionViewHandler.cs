using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Questions.Commands.RegisterQuestionView
{
    public class RegisterQuestionViewHandler : IRequestHandler<RegisterQuestionViewCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<RegisterQuestionViewHandler> _logger;

        public RegisterQuestionViewHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<RegisterQuestionViewHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(RegisterQuestionViewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.Id, true);
                if (question == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                question.RegisterView();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering view for question {QuestionId}", request.Id);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
