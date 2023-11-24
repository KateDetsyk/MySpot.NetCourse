using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Repositories;
using MySpot.tests.Unit.Shared;
using Shouldly;

namespace MySpot.tests.Unit.Services
{
    public class ReservationServiceTests
    {
        [Fact]
        public void gieven_valid_command_create_should_add_reservation()
        {
            // ARRANGE
            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), 
                Guid.NewGuid(), "Joe Doe", "XYZ123", _clock.Current().AddDays(1));

            // ACT
            var reservationId = _reservationService.Create(command);

            // ASSERT
            reservationId.ShouldNotBeNull();
            reservationId.Value.ShouldBe(command.ReservationId);
        }

        [Fact]
        public void gieven_invalid_parking_spot_id_create_should_fail()
        {
            // ARRANGE
            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000010"),
                Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

            // ACT
            var reservationId = _reservationService.Create(command);

            // ASSERT
            reservationId.ShouldBeNull();
        }

        [Fact]
        public void given_reservation_for_already_taken_date_create_should_fail()
        {
            // ARRANGE
            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

            _reservationService.Create(command);

            // ACT
            var reservationId = _reservationService.Create(command);

            // ASSERT
            reservationId.ShouldBeNull();
        }

        #region ARRANGE

        private readonly TestClock _clock;
        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _clock = new TestClock ();
            _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);

            _reservationService = new ReservationService(_clock, _weeklyParkingSpotRepository);
        }

        #endregion
    }
}
