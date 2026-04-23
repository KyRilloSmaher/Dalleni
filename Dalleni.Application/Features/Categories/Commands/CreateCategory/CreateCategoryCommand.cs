using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Name, string? Description) : IRequest<Response<Guid>>;
}
