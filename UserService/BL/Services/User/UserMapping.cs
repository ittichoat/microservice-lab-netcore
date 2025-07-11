using AutoMapper;
using BL.Entities;
using BL.Model;

namespace BL.Mapper
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            // Map RegisterRequest → Domain.Entities.User
            CreateMap<RegisterRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // Map Domain.Entities.User → UserDto
            CreateMap<User, UserDto>();
        }
    }
}