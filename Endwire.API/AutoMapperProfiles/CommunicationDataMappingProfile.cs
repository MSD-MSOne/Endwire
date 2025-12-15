using AutoMapper;
using EndWire.Domain.Models;

namespace EndWire.API.AutoMapperProfiles
{
    public class CommunicationDataMappingProfile : Profile
    {
       public CommunicationDataMappingProfile() 
       {
           //CreateMap<Domain.DTO.NoteDto, Note>().ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id));
       }    
    }
}
