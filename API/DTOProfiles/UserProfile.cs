using AutoMapper;
using Core.Models;
using Core.DTOs.User;

namespace API.DTOProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserRegAndAuthDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
