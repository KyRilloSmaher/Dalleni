using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.Services;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Application.Features.Services.Commands.CreateService;
using Dalleni.Application.Features.Services.Commands.DeleteService;
using Dalleni.Application.Features.Services.Commands.RestoreService;
using Dalleni.Application.Features.Services.Commands.ToggleServiceAvailability;
using Dalleni.Application.Features.Services.Commands.UpdateService;
using Dalleni.Application.Features.Services.Queries;
using Dalleni.Application.Features.Services.Queries.GetAllServices;
using Dalleni.Application.Features.Services.Queries.GetServiceById;
using Dalleni.Application.Features.Services.Queries.GetServicesByCategory;
using Dalleni.Application.Features.Services.Queries.GetServicesByOfficialEntity;
using Dalleni.Application.Features.Services.Queries.SearchServices;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServicesController : BaseController
    {
        public ServicesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get service by ID
        /// </summary>
        [HttpGet(APIROUTES.Services.GetById)]
        [AllowAnonymous] // Public endpoint
        [ProducesResponseType(typeof(Response<ServiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetServiceByIdQuery(id));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get all services (paginated)
        /// </summary>
        [HttpGet(APIROUTES.Services.GetAll)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<PaginatedResult<ServiceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync([FromQuery]PagedRequest request)
        {
            var result = await _mediator.Send(new GetAllServicesQuery(request));
            return FinalResponse(result);
        }

        /// <summary>
        /// Search services by keyword
        /// </summary>
        [HttpGet(APIROUTES.Services.Search)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<IEnumerable<ServiceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAsync([FromQuery] string keyword)
        {
            var result = await _mediator.Send(new SearchServicesQuery(keyword));
            return FinalResponse(result);
        }

        /// <summary>
        /// Get services by category/type 
        /// </summary>
        [HttpGet(APIROUTES.Services.GetByCategory)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<IEnumerable<ServiceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCategoryAsync([FromRoute] Guid categoryId)
        {
            var result = await _mediator.Send(new GetServicesByCategoryQuery(categoryId));
            return FinalResponse(result);
        }


        /// <summary>
        /// Get services by official entity
        /// </summary>
        [HttpGet(APIROUTES.Services.GetByOfficialEntity)]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<IEnumerable<ServiceDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByOfficialEntityAsync([FromRoute] Guid officialEntityId)
        {
            var result = await _mediator.Send(new GetServicesByOfficialEntityQuery(officialEntityId));
            return FinalResponse(result);
        }

        /// <summary>
        /// Create a new service (for official entity owners)
        /// </summary>
        [HttpPost(APIROUTES.Services.Create)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateServiceRequestDto dto)
        {
          var userId = GetCurrentUserId();
            var result = await _mediator.Send(new CreateServiceCommand(dto, userId));
            return FinalResponse(result);
        }

        /// <summary>
        /// Update service details
        /// </summary>
        [HttpPut(APIROUTES.Services.Update)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateServiceRequestDto dto)
        {
               var userId = GetCurrentUserId();
            var result = await _mediator.Send(new UpdateServiceCommand(dto, userId));
            return FinalResponse(result);
        }

        /// <summary>
        /// Delete service (Soft delete)
        /// </summary>
        [HttpDelete(APIROUTES.Services.Delete)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
             var userId = GetCurrentUserId();
            var result = await _mediator.Send(new DeleteServiceCommand(id, userId));
            return FinalResponse(result);
        }

        /// <summary>
        /// Restore a deleted service
        /// </summary>
        [HttpPost(APIROUTES.Services.Restore)]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new RestoreServiceCommand(id,userId));
            return FinalResponse(result);
        }


        /// <summary>
        /// Toggle service availability
        /// </summary>
        [HttpPatch(APIROUTES.Services.ToggleAvailability)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ToggleAvailabilityAsync([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var result = await _mediator.Send(new ToggleServiceAvailabilityCommand(id, userId));
            return FinalResponse(result);
        }

        
    }
}