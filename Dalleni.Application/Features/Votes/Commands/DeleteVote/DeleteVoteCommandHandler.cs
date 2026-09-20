using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Votes.Commands.DeleteVote
{
    public class DeleteVoteCommandHandler : IRequestHandler<DeleteVoteCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<DeleteVoteCommandHandler> _logger;

        public DeleteVoteCommandHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<DeleteVoteCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
        {
          var vote = await _unitOfWork.Votes.GetByIdAsync(request.voteId,true);
          if (vote is null )
          {
            return _responseHandler.NotFound<bool>(SystemMessages.NOT_FOUND);
          }
          else if (vote.UserId != request.UserId)
            {
                return _responseHandler.NotFound<bool>(SystemMessages.UNAUTHORIZED);
            }
         _unitOfWork.Votes.Remove(vote);
         await _unitOfWork.SaveChangesAsync();
         return _responseHandler.Success(true ,SystemMessages.SUCCESS);
        }
    }
}
