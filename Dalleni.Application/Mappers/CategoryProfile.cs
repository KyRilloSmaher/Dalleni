using AutoMapper;
using Dalleni.Application.DTOs.Responses.Categories;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
