using AutoMapper;
using URL_Shortener.BLL.Models;
using URL_Shortener.DAL.Entities;

namespace URL_Shortener.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {           
            CreateMap<UserAccount, UserDTO>().ReverseMap().ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<UrlDTO, Url>().ReverseMap().ForMember(dest => dest.UserId, opt => opt.MapFrom(x => x.UserAccountId));
        }
    }
}
