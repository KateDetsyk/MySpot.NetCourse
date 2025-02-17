﻿using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed class CannotReserveParkingSpotException : CustomExcption
    {
        public ParkingSpotId ParkingSpotId { get; }

        public CannotReserveParkingSpotException(ParkingSpotId parkingSpotId) 
            : base($"Cannot reserve parking spot with ID: {parkingSpotId} due to the reservation policy.")
        {
            ParkingSpotId = parkingSpotId;
        }
    }
}
