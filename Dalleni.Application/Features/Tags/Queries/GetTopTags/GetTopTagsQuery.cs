using Dalleni.Application.DTOs.Requests.Base;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Tags.Queries.GetTopTags
{
    public record GetTopTagsQuery(PagedRequest Request) : IRequest<Response<PaginatedResult<TagDto>>>;
}
