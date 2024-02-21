using MySpot.Application.DTO;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Handlers
{
    public static class Extentions
    {
        public static WeeklyParkingSpotDto AsDto(this WeeklyParkingSpot entity)
            => new()
            {
                Id = entity.Id.Value.ToString(),
                Name = entity.Name.Value,
                Capacity = entity.Capacity.Value,
                From = entity.Week.From.Value.DateTime,
                To = entity.Week.To.Value.DateTime,
                Reservations = entity.Reservations.Select(x => new ReservationDto
                {
                    Id = x.Id,
                    EmployeeName = x is VehicleReservation vr ? vr.EmployeeName : null,
                    Date = x.Date.Value.Date
                })
            };
    }
}
