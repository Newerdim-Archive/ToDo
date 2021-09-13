using AutoMapper;
using ToDo.API.Entities;

namespace ToDo.API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, Dto.User>();
        }
    }
}