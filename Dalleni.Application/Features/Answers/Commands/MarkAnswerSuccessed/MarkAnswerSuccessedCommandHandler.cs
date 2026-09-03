using Dalleni.Application.Features.Answers.Commands.MarkAnswerSuccessed;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Answers.Commands.MarkAnswerAsSuccessed
{
    public record MarkAnswerAsSuccessedCommandHandler : IRequestHandler<MarkAnswerSuccessedCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public MarkAnswerAsSuccessedCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        public async Task<Response<bool>> Handle(MarkAnswerSuccessedCommand request, CancellationToken cancellationToken)
        {
            var answer = await _unitOfWork.Answers.GetByIdAsync(request.id, true, cancellationToken);

            if (answer == null || answer.IsDeleted)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }
            if (answer.UserId == request.userId)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.CANNOT_MARK_OWN_ANSWER);
            }

            answer.MarkAsSuccessful();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _responseHandler.Success(true, SystemMessages.SUCCESS);

      }
    }
}
