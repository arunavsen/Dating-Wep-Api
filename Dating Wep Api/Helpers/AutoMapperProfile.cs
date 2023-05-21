using AutoMapper;
using Dating_Wep_Api.DTO;
using Dating_Wep_Api.Models;

namespace Dating_Wep_Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserForListDTO>();
            CreateMap<User, UserForDetailsDTO>();
        }
    }
}
