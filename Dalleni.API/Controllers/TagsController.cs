using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.Features.Tags.Queries;
using Dalleni.Application.Features.Tags.Queries.GetTopTags;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
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
    }
}
