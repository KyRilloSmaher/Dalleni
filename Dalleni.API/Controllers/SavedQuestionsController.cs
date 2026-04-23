using Dalleni.API.Bases;
using Dalleni.Application.Features.SavedQuestions.Commands.AddSavedQuestion;
using Dalleni.Application.Features.SavedQuestions.Commands.DeleteSavedQuestion;
using Dalleni.Application.Features.SavedQuestions.Queries.GetUserSavedQuestion;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models;
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
    public class SavedQuestionsController : BaseController
    {
        public SavedQuestionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(APIROUTES.savedQuestions.GetAll)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<IEnumerable<SavedQuestion>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllForUserasync()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var parseduserId = Guid.Parse(userId);
            var result = await _mediator.Send(new GetUserSavedQuestionsQuery(parseduserId));
            return FinalResponse(result);
        }
        [HttpPost(APIROUTES.savedQuestions.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<SavedQuestion>), StatusCodes.Status200OK)]

        public async Task<IActionResult> CreateAsync([FromBody] AddSavedQuestionCommand command)
        {
            var result = await _mediator.Send(command);
            return FinalResponse(result);

        }
        [HttpPost(APIROUTES.savedQuestions.Remove)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]

        public async Task<IActionResult> RemoveAsync([FromRoute]  Guid id)
        {
            var result = await _mediator.Send(new DeleteSavedQuestionCommand(id));
            return FinalResponse(result);

        }
    }
}
