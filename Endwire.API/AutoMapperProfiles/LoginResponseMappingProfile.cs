using AutoMapper;
using EndWire.Domain;
using EndWire.Domain.Models;

namespace EndWire.API.AutoMapperProfiles
{
    public class LoginResponseMappingProfile: Profile
    {
       public LoginResponseMappingProfile() 
       {
           CreateMap<LoginResponseModel, LoginResponse>().ReverseMap();
       }    
    }
}
