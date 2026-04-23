using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Application.Features.Categories.Queries;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(APIROUTES.Categories.GetAll)]
        [ProducesResponseType(typeof(Response<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return FinalResponse(result);
        }
    }
}
