using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Application.Features.Votes.Commands.DeleteVote;
using Dalleni.Application.Features.Votes.Commands.VoteAnswer;
using Dalleni.Application.Features.Votes.Commands.VoteQuestion;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedAnswersQuery;
using Dalleni.Application.Features.Votes.Queries.GetUserVotedQuestionsQuery;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    public class VotesController : BaseController
    {
        public VotesController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet(APIROUTES.Votes.GetUserVotedQuestions)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<IEnumerable<VotedQuestionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserVotedQuestions()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new GetUserVotedQuestionsQuery(userId));
            return FinalResponse(result);
        }

        [HttpGet(APIROUTES.Votes.GetUserVotedAnswers)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<IEnumerable<VotedAnswerResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserVotedAnswers()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new GetUserVotedAnswersQuery(userId));
            return FinalResponse(result);
        }

        [HttpPost(APIROUTES.Votes.VoteQuestion)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<NewVoteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VoteQuestionAsync([FromRoute] Guid id, [FromQuery] VoteType type)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new VoteQuestionCommand(id, userId, type));
            return FinalResponse(result);
        }

        [HttpPost(APIROUTES.Votes.VoteAnswer)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<NewVoteResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VoteAnswerAsync([FromRoute] Guid id, [FromQuery] VoteType type)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new VoteAnswerCommand(id, userId, type));
            return FinalResponse(result);
        }

        [HttpDelete(APIROUTES.Votes.RemoveVote)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteVoteAsync([FromRoute] Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new DeleteVoteCommand(userId,id));
            return FinalResponse(result);
        }
    }
}
