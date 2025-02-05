using Core.DTOs.ToDo;
using AutoMapper;
using Core.Models;

namespace API.DTOProfiles
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDoGetByDateAndTimeBlockDto, ToDo>();
            CreateMap<ToDoDto, ToDo>();
            CreateMap<ToDoAddDto, ToDo>();
            CreateMap<ToDoUpdateDto, ToDo>();
        }
    }
}
