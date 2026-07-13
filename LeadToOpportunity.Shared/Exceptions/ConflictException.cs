namespace LeadToOpportunity.Shared.Exceptions;

public class ConflictException : System.Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}