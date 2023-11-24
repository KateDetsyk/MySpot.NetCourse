using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories
{
    internal sealed class PostgresWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
    {
        public void Add(WeeklyParkingSpot weeklyParkingSpot)
        {
            throw new NotImplementedException();
        }

        public WeeklyParkingSpot Get(ParkingSpotId id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeeklyParkingSpot> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(WeeklyParkingSpot weeklyParkingSpot)
        {
            throw new NotImplementedException();
        }
    }
}
