﻿using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IClock _clock;

        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;

        public ReservationService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepository)
        {
            _clock = clock;
            _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
        }

        public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
            => (await _weeklyParkingSpotRepository
                .GetAllAsync())
                .SelectMany(x => x.Reservations)
                .Select(x => new ReservationDto
                {
                    Id = x.Id,
                    EmployeeName = x.EmployeeName,
                    Date = x.Date.Value.Date
                });

        public async Task<ReservationDto> GetAsync(Guid id)
            => (await GetAllWeeklyAsync()).SingleOrDefault(x => x.Id == id);

        public async Task CreateAsync(CreateReservation command)
        {
            var (spotId, reservationId, employeeName, licencePlate, date) = command;

            var weeklyParkingSpot = await _weeklyParkingSpotRepository.GetAsync(spotId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException(spotId);
            }

            var reservation = new Reservation(reservationId, employeeName, licencePlate, new Date(date));

            weeklyParkingSpot.AddReservation(reservation, new Date(CurrentDate()));
            await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);
        }

        public async Task UpdateAsync(ChangeReservationLicencePlate command)
        {
            var weeklyParkingSpot = await GetWeeklyParkingSpotByReservation(command.ReservationId);

            if (weeklyParkingSpot is null)
            {
                throw new WeeklyParkingSpotNotFoundException();
            }

            var reservation = weeklyParkingSpot.Reservations
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
