using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed class ParkingSpotCapacityExceededException : CustomExcption
    {
        public ParkingSpotId ParkingSpotId { get; }

        public ParkingSpotCapacityExceededException(ParkingSpotId parkingSpotId)
            : base($"Parking Spot with ID: {parkingSpotId} exceeded its reservation capacity.")
        {
            ParkingSpotId = parkingSpotId;
        }
    }
}
