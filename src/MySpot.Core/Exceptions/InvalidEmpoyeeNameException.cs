namespace MySpot.Core.Exceptions
{
    public class InvalidEmpoyeeNameException : CustomException
    {
        public InvalidEmpoyeeNameException() : base($"Employee name is invalid.")
        {
        }
    }
}
