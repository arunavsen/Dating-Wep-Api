using AutoMapper;
using Dating_Wep_Api.DTO;
using Dating_Wep_Api.Models;

namespace Dating_Wep_Api.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<User, UserForDetailsDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotosForDetailsDTO>();
            CreateMap<UserForUpdateDTO, User>();
        }
    }
}
