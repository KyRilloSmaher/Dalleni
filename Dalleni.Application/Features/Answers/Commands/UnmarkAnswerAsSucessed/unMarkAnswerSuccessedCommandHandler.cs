using Dalleni.Application.Features.Answers.Commands.MarkAnswerSuccessed;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Answers.Commands.UnmarkAnswerAsSuccessed
{
    public record UnmarkAnswerAsSuccessedCommandHandler : IRequestHandler<UnmarkAnswerAsSuccessedCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public UnmarkAnswerAsSuccessedCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        public async Task<Response<bool>> Handle(UnmarkAnswerAsSuccessedCommand request, CancellationToken cancellationToken)
        {
            var answer = await _unitOfWork.Answers.GetByIdAsync(request.id);

            if (answer == null || answer.IsDeleted)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }
            if (answer.UserId == request.userId)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.CANNOT_UNMARK_OWN_ANSWER);
            }

            answer.UnmarkAsSuccessful();

            return _responseHandler.Success(true, SystemMessages.SUCCESS);

      }
    }
}