namespace MySpot.Core.Exceptions;

public abstract class CustomExcption : Exception
{
    protected CustomExcption(string message) : base(message) 
    { 
    }
}
