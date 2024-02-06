using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services
{
    public interface IReservationService
    {
        Task CreateAsync(CreateReservation command);
        Task DeleteAsync(DeleteReservation command);
        Task<ReservationDto> GetAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
        Task UpdateAsync(ChangeReservationLicencePlate command);
    }
}