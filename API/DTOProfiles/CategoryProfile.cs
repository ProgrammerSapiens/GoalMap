using AutoMapper;
using Core.DTOs.ToDoCategory;
using Core.Models;

namespace API.DTOProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping ToDo category DTOs to domain models.
    /// </summary>
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Initializes the mapping configuration for ToDo categories.
        /// </summary>
        public CategoryProfile()
        {
            CreateMap<CategoryUpdateDto, ToDoCategory>().ReverseMap();
            CreateMap<CategoryAddDto, ToDoCategory>().ReverseMap();
            CreateMap<CategoryDto, ToDoCategory>().ReverseMap();
        }
    }
}
