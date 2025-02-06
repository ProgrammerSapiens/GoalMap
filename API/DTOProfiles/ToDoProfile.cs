using Core.DTOs.ToDo;
using AutoMapper;
using Core.Models;

namespace API.DTOProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping ToDo-related DTOs to domain models.
    /// </summary>
    public class ToDoProfile : Profile
    {
        /// <summary>
        /// Initializes the mapping configuration for ToDo entities.
        /// </summary>
        public ToDoProfile()
        {
            CreateMap<ToDoGetByDateAndTimeBlockDto, ToDo>();
            CreateMap<ToDoDto, ToDo>();
            CreateMap<ToDoAddDto, ToDo>();
            CreateMap<ToDoUpdateDto, ToDo>();
        }
    }
}
