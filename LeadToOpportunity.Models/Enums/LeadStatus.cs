using System.Data;
using System.Runtime.InteropServices;

namespace LeadToOpportunity.Models.Enums;

public enum LeadStatus
{
    Draft,
    Submittted,
    UnderReview,
    ModificationRequested,
    Approved,
    Rejected,
    Converted
    
}