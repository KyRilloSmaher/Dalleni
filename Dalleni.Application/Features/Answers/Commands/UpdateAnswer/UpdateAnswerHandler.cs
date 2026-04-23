using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Answers.Commands.UpdateAnswer
{
    public class UpdateAnswerHandler : IRequestHandler<UpdateAnswerCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<UpdateAnswerHandler> _logger;

        public UpdateAnswerHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<UpdateAnswerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(UpdateAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = await _unitOfWork.Answers.GetByIdAsync(request.Id, true);
                if (answer == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                if (answer.UserId != request.UserId)
                    return _responseHandler.Unauthorized<bool>(SystemMessages.ACCESS_DENIED);

                answer.Update(request.dto.Content);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.RECORD_UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating answer {AnswerId}", request.Id);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
