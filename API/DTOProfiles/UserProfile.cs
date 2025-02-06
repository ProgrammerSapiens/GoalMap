using AutoMapper;
using Core.Models;
using Core.DTOs.User;

namespace API.DTOProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping user DTOs to domain models.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Initializes the mapping configuration for User entities.
        /// </summary>
        public UserProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<UserRegAndAuthDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
