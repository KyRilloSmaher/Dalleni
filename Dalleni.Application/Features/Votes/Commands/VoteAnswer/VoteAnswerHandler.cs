using Dalleni.Application.DTOs.Responses.Votes;
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
    public class VoteAnswerHandler : IRequestHandler<VoteAnswerCommand, Response<NewVoteResponse>>
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

        public async Task<Response<NewVoteResponse>> Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var answer = await _unitOfWork.Answers.GetByIdAsync(request.AnswerId, true);
                if (answer == null)
                    return _responseHandler.NotFound<NewVoteResponse>(SystemMessages.RECORD_NOT_FOUND);

                if (answer.UserId == request.UserId)
                    return _responseHandler.BadRequest<NewVoteResponse>(SystemMessages.CANNOT_VOTE_OWN_ANSWER);

                var existingVote = await _unitOfWork.Votes.GetUserVoteForAnswerAsync(request.UserId, request.AnswerId, true, cancellationToken);
                Vote? vote = null;

                if (existingVote != null)
                {
                    if (existingVote.Type == request.Type)
                    {
                        return _responseHandler.BadRequest<NewVoteResponse>(
                            SystemMessages.AlREADY_VOTED);
                    }

                    vote = existingVote;

                    if (existingVote.Type == VoteType.Downvote)
                        answer.DecreaseVote(VoteType.Downvote);
                    else
                        answer.DecreaseVote(VoteType.Upvote);

                    existingVote.UpdateType(request.Type);
                }
                else
                {
                    vote = Vote.Create(
                        request.UserId,
                        request.Type,
                        answerId: request.AnswerId);

                    await _unitOfWork.Votes.AddAsync(vote);
                }

                answer.ApplyVote(request.Type);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var answerAfterCommit =
                    await _unitOfWork.Answers.GetByIdAsync(
                        request.AnswerId,
                        false);

                var dto = NewVoteResponse.Create(
                    voteId: vote.Id,
                    newDownvotesCount: answerAfterCommit.DownVotes,
                    newUpvotesCount: answerAfterCommit.UpVotes
                );

                return _responseHandler.Success(dto, SystemMessages.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error voting on answer {AnswerId}", request.AnswerId);
                return _responseHandler.BadRequest<NewVoteResponse>(ex.Message);
            }
        }
    }
}
