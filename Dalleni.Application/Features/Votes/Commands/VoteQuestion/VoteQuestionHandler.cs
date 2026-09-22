using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Interfaces.Handlers;
using Dalleni.Domin.Interfaces.Repositories;
using Dalleni.Domin.Models;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dalleni.Application.Features.Votes.Commands.VoteQuestion
{
    public class VoteQuestionHandler : IRequestHandler<VoteQuestionCommand, Response<NewVoteResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseHandler _responseHandler;
        private readonly ILogger<VoteQuestionHandler> _logger;

        public VoteQuestionHandler(IUnitOfWork unitOfWork, IResponseHandler responseHandler, ILogger<VoteQuestionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _responseHandler = responseHandler;
            _logger = logger;
        }

        public async Task<Response<NewVoteResponse>> Handle(VoteQuestionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var question = await _unitOfWork.Questions.GetByIdAsync(request.QuestionId, true);
                if (question == null)
                    return _responseHandler.NotFound<NewVoteResponse>(SystemMessages.RECORD_NOT_FOUND);

                if (question.UserId == request.UserId)
                    return _responseHandler.BadRequest<NewVoteResponse>(SystemMessages.CANNOT_VOTE_OWN_QUESTION);

                var existingVote = await _unitOfWork.Votes.GetUserVoteForQuestionAsync(request.UserId, request.QuestionId, true, cancellationToken);
                Vote? vote = null;

                if (existingVote != null)
                {
                    if (existingVote.Type == request.Type)
                    {
                        return _responseHandler.BadRequest<NewVoteResponse>(SystemMessages.AlREADY_VOTED);
                    }

                    vote = existingVote;

                    if (existingVote.Type == VoteType.Downvote)
                        question.DecreaseVote(VoteType.Downvote);
                    else
                        question.DecreaseVote(VoteType.Upvote);

                    existingVote.UpdateType(request.Type);
                }
                else
                {
                    vote = Vote.Create(
                        request.UserId,
                        request.Type,
                        questionId: request.QuestionId);

                    await _unitOfWork.Votes.AddAsync(vote);
                }

                question.ApplyVote(request.Type);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var questionAfterCommit =
                    await _unitOfWork.Questions.GetByIdAsync(
                        request.QuestionId,
                        false);

                var dto = NewVoteResponse.Create(
                    voteId: vote.Id,
                    newDownvotesCount: questionAfterCommit.DownVotes,
                    newUpvotesCount: questionAfterCommit.UpVotes
                );


                return _responseHandler.Success(dto, SystemMessages.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error voting on question {QuestionId}", request.QuestionId);
                return _responseHandler.BadRequest<NewVoteResponse>(ex.Message);
            }
        }
    }
}
