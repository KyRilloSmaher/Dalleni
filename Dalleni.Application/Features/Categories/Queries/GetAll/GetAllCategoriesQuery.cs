using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Domin.ResponsePattern;
using MediatR;

namespace Dalleni.Application.Features.Categories.Queries.GetAll
{
    public record GetAllCategoriesQuery() : IRequest<Response<IEnumerable<CategoryDto>>>;
}
