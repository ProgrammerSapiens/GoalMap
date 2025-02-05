using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Models;

namespace API.DTOProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddOrUpdateDto, ToDoCategory>();
        }
    }
}
