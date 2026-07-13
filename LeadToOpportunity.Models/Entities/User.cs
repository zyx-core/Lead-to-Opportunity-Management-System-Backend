
using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class User : BaseEntity
{
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get; set;}= string.Empty;

    public string Email {get; set;} = string.Empty;

    public string PasswordHash{get; set;} = string.Empty;

    public UserRole Role{get;set;}

    public string Region {get; set;} = string.Empty;

    //navigations

    public ICollection<Lead> CreatedLeads {get; set;} = new List<Lead>();
    public ICollection<Lead> AssignedLeads {get; set;} = new List<Lead>();
    

}