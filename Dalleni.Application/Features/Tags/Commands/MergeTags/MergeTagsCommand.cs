using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Tags.Commands.MergeTags
{
    public record MergeTagsCommand(Guid SourceTagId, Guid TargetTagId) : IRequest<Response<bool>>;
}
