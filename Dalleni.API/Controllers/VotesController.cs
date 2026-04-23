using Dalleni.API.Bases;
using Dalleni.Application.Features.Votes.Commands.VoteAnswer;
using Dalleni.Application.Features.Votes.Commands.VoteQuestion;
using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpPost(APIROUTES.Votes.VoteQuestion)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VoteQuestionAsync([FromRoute] Guid id, [FromQuery] VoteType type)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new VoteQuestionCommand(id, userId, type));
            return FinalResponse(result);
        }

        [HttpPost(APIROUTES.Votes.VoteAnswer)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VoteAnswerAsync([FromRoute] Guid id, [FromQuery] VoteType type)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new VoteAnswerCommand(id, userId, type));
            return FinalResponse(result);
        }
    }
}
