using AutoMapper;
using LeadToOpportunity.Models.Entities;


namespace LeadToOpportunity.BLL.Mappings;

public class UserProfile : Profile
{
    public UserProfile(){
     CreateMap<RegisterRequestDto, User >  (); 
    }
}