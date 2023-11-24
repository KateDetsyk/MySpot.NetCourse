namespace MySpot.Core.Exceptions
{
    public class InvalidEmpoyeeNameException : CustomExcption
    {
        public InvalidEmpoyeeNameException() : base($"Employee name is invalid.")
        {
        }
    }
}
