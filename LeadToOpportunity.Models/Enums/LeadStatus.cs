using System.Data;
using System.Runtime.InteropServices;

namespace LeadToOpportunity.Models.Enums;

public enum LeadStatus
{
    Draft,
    Submitted,
    UnderReview,
    ModificationRequested,
    Approved,
    Rejected,
    Converted
    
}