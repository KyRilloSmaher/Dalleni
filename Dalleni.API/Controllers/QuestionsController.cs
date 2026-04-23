using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Requests.Questions;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.Features.Questions.Commands.AcceptAnswer;
using Dalleni.Application.Features.Questions.Commands.CloseQuestion;
using Dalleni.Application.Features.Questions.Commands.CreateQuestion;
using Dalleni.Application.Features.Questions.Commands.DeleteQuestion;
using Dalleni.Application.Features.Questions.Commands.UpdateQuestion;
using Dalleni.Application.Features.Questions.Queries;
using Dalleni.Application.Features.Questions.Queries.GetByTag;
using Dalleni.Application.Features.Questions.Queries.GetPagedQuestions;
using Dalleni.Application.Features.Questions.Queries.GetQuestionDetails;
using Dalleni.Application.Features.Questions.Queries.GetRelatedQuestions;
using Dalleni.Application.Features.Questions.Queries.GetSimilars;
using Dalleni.Application.Features.Questions.Queries.Search;
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
    public class QuestionsController : BaseController
    {
        public QuestionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(APIROUTES.Questions.GetById)]
        [ProducesResponseType(typeof(Response<QuestionDetailsResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetQuestionDetailsQuery(id));
            return FinalResponse(result);
        }

        [HttpGet(APIROUTES.Questions.GetAllPaged)]
        [ProducesResponseType(typeof(Response<PaginatedResult<QuestionSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPagedAsync([FromQuery] PagedRequest request)
        {
            var result = await _mediator.Send(new GetPagedQuestionsQuery(request));
            return FinalResponse(result);
        }
        [HttpGet(APIROUTES.Questions.GetByTag)]
        [ProducesResponseType(typeof(Response<PaginatedResult<QuestionSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByTagAsync([FromQuery] PagedRequest request , [FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetByTagQuery(id ,request));
            return FinalResponse(result);
        }
        [HttpGet(APIROUTES.Questions.Search)]
        [ProducesResponseType(typeof(Response<PaginatedResult<QuestionSummaryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchAsync([FromQuery] PagedRequest request, [FromQuery] string query)
        {
            var result = await _mediator.Send(new SearchQuery(query ,request));
            return FinalResponse(result);
        }
        [HttpGet(APIROUTES.Questions.Related)]
        [ProducesResponseType(typeof(Response<IEnumerable<QuestionDetailsResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RelatedQuestionsAsync([FromRoute] Guid id , [FromQuery] int count)
        {
            if(count <= 0)
                count = 5;
            var result = await _mediator.Send(new GetRelatedQuestionsQuery(id ,count));
            return FinalResponse(result);
        }
        [HttpGet(APIROUTES.Questions.Similars)]
        [ProducesResponseType(typeof(Response<IEnumerable<QuestionDetailsResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SimilarsQuestionaAsync([FromQuery] string Question)
        {
            var result = await _mediator.Send(new SimilarQuestionsQuery(Question));
            return FinalResponse(result);
        }

        [HttpPost(APIROUTES.Questions.Close)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CloseAsync([FromRoute] Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new CloseQuestionCommand(id, userId));
            return FinalResponse(result);
        }

        [HttpPost(APIROUTES.Questions.AcceptAnswer)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AcceptAnswerAsync([FromRoute] Guid id, [FromQuery] Guid questionId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new AcceptAnswerCommand(questionId, id, userId));
            return FinalResponse(result);
        }
        [HttpPost(APIROUTES.Questions.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateQuestionRequestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new CreateQuestionCommand(dto, userId));
            return FinalResponse(result);
        }

        [HttpPut(APIROUTES.Questions.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateQuestionRequestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new UpdateQuestionCommand(id, dto, userId));
            return FinalResponse(result);
        }

        [HttpDelete(APIROUTES.Questions.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _mediator.Send(new DeleteQuestionCommand(id, userId));
            return FinalResponse(result);
        }
    }
}
