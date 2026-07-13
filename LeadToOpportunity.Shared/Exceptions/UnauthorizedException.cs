namespace LeadToOpportunity.Shared.Exceptions;

public class UnauthorizedException : System.Exception
{
    public UnauthorizedException(string message)
        : base(message)
    {
    }
}