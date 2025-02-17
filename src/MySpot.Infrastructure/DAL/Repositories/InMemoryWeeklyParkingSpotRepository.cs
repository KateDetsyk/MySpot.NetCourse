﻿using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
    {
        private readonly List<WeeklyParkingSpot> _weeklyParkingSpots;

        public InMemoryWeeklyParkingSpotRepository(IClock clock)
        {
            _weeklyParkingSpots = new List<WeeklyParkingSpot> {
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(clock.Current()), "P3"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(clock.Current()), "P2"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(clock.Current()), "P4"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(clock.Current()), "P1"),
                WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(clock.Current()), "P5"),
            };
        }

        public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _weeklyParkingSpots;
        }

        public async Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id)
        {
            // TODO I added .value to id, why it doesn't work without it
            await Task.CompletedTask;
            return _weeklyParkingSpots.SingleOrDefault(x => x.Id == id.Value);
        }

        public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
        {
            _weeklyParkingSpots?.Add(weeklyParkingSpot);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot) => Task.CompletedTask;
    }
}
