using AutoMapper;
using EndWire.Domain;
using EndWire.Domain.Models;

namespace EndWire.API.AutoMapperProfiles
{
    public class UserLocationMappingProfile : Profile
    {
       public UserLocationMappingProfile() 
       {
           CreateMap<Loc, UserLocationResult>().ForMember(dest=>dest.LocationId, opt=>opt.MapFrom(src=>src.Id)).ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Name)).ReverseMap();
           CreateMap<GroupDto, GroupResult>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.Name)).ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId)).ReverseMap();
        }    
    }
}
