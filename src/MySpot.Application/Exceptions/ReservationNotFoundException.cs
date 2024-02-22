using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions
{
    public sealed class ReservationNotFoundException : CustomExcption
    {
        public Guid Id { get; }

        public ReservationNotFoundException(Guid id)
            : base($"Reservation with ID: {id} was not found.")
        {
            Id = id;
        }
    }
}
