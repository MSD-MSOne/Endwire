using AutoMapper;
using EndWire.Domain;
using EndWire.Domain.Models;

namespace EndWire.API.AutoMapperProfiles
{
    public class NoteMappingProfile: Profile
    {
       public NoteMappingProfile() 
       {
           CreateMap<NoteDto, Note>().ReverseMap();
       }    
    }
}
