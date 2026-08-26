using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Answers.Commands.AcceptAnswer
{
    public record AcceptAnswerCommandHandler : IRequestHandler<AcceptAnswerCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;

        public AcceptAnswerCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
        }

        public async Task<Response<bool>> Handle(AcceptAnswerCommand request, CancellationToken cancellationToken)
        {
            var answer = await _unitOfWork.Answers.GetByIdAsync(request.id);

            if (answer == null || answer.IsDeleted)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
            }
            if (answer.UserId == request.userId)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.CANNOT_UNACCEPT_OWN_ANSWER);
            }
            if (answer.IsAccepted)
            {
                return _responseHandler.BadRequest<bool>(SystemMessages.ANSWER_ALREADY_ACCEPTED);
            }


            answer.Accept();

            await _unitOfWork.SaveChangesAsync();

            return _responseHandler.Success(true, SystemMessages.SUCCESS);
        }
    }
}