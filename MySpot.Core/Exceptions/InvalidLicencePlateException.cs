namespace MySpot.Core.Exceptions
{
    public class InvalidLicencePlateException : CustomExcption
    {
        public string LicencePlate { get; set; }

        public InvalidLicencePlateException(string licencePlate)
            : base($"Licence plate: {licencePlate} is invalid.")
        {
            LicencePlate = licencePlate;
        }
    }
}
