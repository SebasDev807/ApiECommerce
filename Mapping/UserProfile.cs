using ApiEcommerce.Models;
using ApiEcommerce.Models.DTOs;
using AutoMapper;

namespace ApiEcommerce.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserRegisterDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
        CreateMap<User, UserResponseDto>().ReverseMap();
    }
}
