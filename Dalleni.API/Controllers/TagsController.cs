using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.Features.Questions.Queries.GetByTag;
using Dalleni.Application.Features.Tags.Queries;
using Dalleni.Application.Features.Tags.Queries.GetTopTags;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : BaseController
    {
        public TagsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(APIROUTES.Tags.GetAll)]
        [ProducesResponseType(typeof(Response<IEnumerable<TagDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagedRequest request)
        {
            var result = await _mediator.Send(new GetTopTagsQuery(request));
            return FinalResponse(result);
        }

        [HttpGet(APIROUTES.Tags.GetQuestions)]
        [ProducesResponseType(typeof(Response<PaginatedResult<QuestionSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTagAsync([FromQuery] PagedRequest request , [FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new GetByTagQuery(id ,request,userId));
            return FinalResponse(result);
        }
    }
}
