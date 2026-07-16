namespace LeadToOpportunity.Models.Enums;

public abstract class BaseEntity
{
    public int Id {get; set;}
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow.AddHours(5).AddMinutes(30);

    public DateTime? UpdatedAt {get; set;}
}