using AutoMapper;
using Mongo.Database.Models;
using Mongo.DTOs;

namespace Mongo.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<UserUpdateDTO, User>();
            CreateMap<User, UserDisplayDTO>();
        }
    }
}
