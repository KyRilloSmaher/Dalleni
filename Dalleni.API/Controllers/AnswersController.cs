using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Answers;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.Features.Answers.Commands.CreateAnswer;
using Dalleni.Application.Features.Answers.Commands.DeleteAnswer;
using Dalleni.Application.Features.Answers.Commands.UpdateAnswer;
using Dalleni.Application.Features.Answers.Queries;
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
    public class AnswersController : BaseController
    {
        public AnswersController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet(APIROUTES.Answers.GetByQuestionId)]
        [ProducesResponseType(typeof(Response<IEnumerable<AnswerDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByQuestionIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetAnswersByQuestionIdQuery(id));
            return FinalResponse(result);
        }
        [HttpPost(APIROUTES.Answers.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAnswerRequestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new CreateAnswerCommand(dto, userId));
            return FinalResponse(result);
        }

        [HttpPut(APIROUTES.Answers.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateAnswerRequestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new UpdateAnswerCommand(id, dto, userId));
            return FinalResponse(result);
        }

        [HttpDelete(APIROUTES.Answers.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new DeleteAnswerCommand(id, userId));
            return FinalResponse(result);
        }


    }
}
