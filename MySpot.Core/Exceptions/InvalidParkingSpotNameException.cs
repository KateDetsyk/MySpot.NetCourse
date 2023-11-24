namespace MySpot.Core.Exceptions
{
    public class InvalidParkingSpotNameException : CustomExcption
    {
        public InvalidParkingSpotNameException() : base($"Parking spot name is invalid.")
        {
        }
    }
}
