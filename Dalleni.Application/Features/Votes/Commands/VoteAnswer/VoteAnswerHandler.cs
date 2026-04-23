using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Votes.Commands.VoteAnswer
{
    public class VoteAnswerHandler : IRequestHandler<VoteAnswerCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<VoteAnswerHandler> _logger;

        public VoteAnswerHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<VoteAnswerHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = await _unitOfWork.Answers.GetByIdAsync(request.AnswerId, true);
                if (answer == null)
                    return _responseHandler.NotFound<bool>(SystemMessages.RECORD_NOT_FOUND);

                if (answer.UserId == request.UserId)
                    return _responseHandler.BadRequest<bool>("You cannot vote on your own answer.");

                var existingVote = await _unitOfWork.Votes.GetUserVoteForAnswerAsync(request.UserId, request.AnswerId, true, cancellationToken);

                if (existingVote != null)
                {
                    if (existingVote.Type == request.Type)
                    {
                        _unitOfWork.Votes.Remove(existingVote);
                    }
                    else
                    {
                        existingVote.UpdateType(request.Type);
                    }
                }
                else
                {
                    var vote = Vote.Create(request.UserId, request.Type, answerId: request.AnswerId);
                    await _unitOfWork.Votes.AddAsync(vote);
                }

                answer.ApplyVote(request.Type);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return _responseHandler.Success(true, SystemMessages.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error voting on answer {AnswerId}", request.AnswerId);
                return _responseHandler.BadRequest<bool>(ex.Message);
            }
        }
    }
}
