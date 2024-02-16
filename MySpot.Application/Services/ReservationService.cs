using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IClock _clock;
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
        private readonly IParkingReservationService _parkingReservationService;

        public ReservationService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository, 
            IParkingReservationService parkingReservationService)
        {
            _clock = clock;
            _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
            _parkingReservationService = parkingReservationService;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
            => (await _weeklyParkingSpotRepository
                .GetAllAsync())
                .SelectMany(x => x.Reservations)
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    EmployeeName = x is VehicleReservation vr ? vr.EmployeeName : null,
                    Date = x.Date.Value.Date
                });

        public async Task<ReservationDto> GetAsync(Guid id)
            => (await GetAllWeeklyAsync()).SingleOrDefault(x => x.Id == id);

        public async Task ReserveForVehicleAsync(ReserveParkingSpotForVehicle command)
        {
            var (spotId, reservationId, employeeName, licencePlate, capacity, date) = command;

            var week = new Week(_clock.Current());
            var parkingSpotId = new ParkingSpotId(spotId);
            var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();
            var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id.Value == parkingSpotId.Value);

            if (parkingSpotToReserve is null)
            {
                throw new WeeklyParkingSpotNotFoundException(spotId);
            }

            var reservation = new VehicleReservation(reservationId, employeeName, licencePlate, 
                capacity, new Date(date));

            _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, 
                parkingSpotToReserve, reservation);

            await _weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);
        }

        public async Task ReserveForVehicleCleaningAsync(ReserveParkingSpotForCleaning command)
        {
            var week = new Week(command.Date);
            var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();

            _parkingReservationService.ReserveParkingForCleaning(weeklyParkingSpots, new Date(command.Date));
            
            foreach(var parkingSpot in weeklyParkingSpots)
            {
                await _weeklyParkingSpotRepository.UpdateAsync(parkingSpot);
            }
        }

        public async Task ChangeReservationLicencePlateAsync(ChangeReservationLicencePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException();
            }

            var reservation = weeklyParkingSpot.Reservations
                .OfType<VehicleReservation>()
                .SingleOrDefault(x => x.Id == command.ReservationId);

            if (reservation is null)
            {
                throw new ReservationNotFoundException(command.ReservationId); 
            }

            reservation.ChangeLicencePlate(command.LicencePlate);
            await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
        }

        public async Task DeleteAsync(DeleteReservation command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException();
            }

            weeklyParkingSpot.RemoveReservation(command.ReservationId);
            await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
        }

        private async Task<WeeklyParkingSpot> GetWeeklyParkingSpotByReservation(Guid id)
            => (await _weeklyParkingSpotRepository.GetAllAsync())
                .SingleOrDefault(x => x.Reservations.Any(r => r.Id == id));

        private DateTime CurrentDate() => _clock.Current();
    }
}
