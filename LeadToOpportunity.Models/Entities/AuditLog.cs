using LeadToOpportunity.Models.Enums;

namespace LeadToOpportunity.Models.Entities;

public class AuditLog : BaseEntity
{
    public string EntityType {get;set;} =string.Empty;
    public int EntityId {get;set;}
    public int ActorId{get;set;} 
    public User Actor {get;set;} =null!;
    public  string Action {get;set;} =string.Empty;
    public string FromStatus {get;set;} =string.Empty;
    public string ToStatus {get;set;} = string.Empty;
    public string Comment {get;set;} = string.Empty;
    
    }