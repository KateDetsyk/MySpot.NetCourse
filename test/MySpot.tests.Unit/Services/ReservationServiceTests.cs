//using MySpot.Application.Commands;
//using MySpot.Application.Services;
//using MySpot.Core.Repositories;
//using MySpot.Infrastructure.DAL.Repositories;
//using MySpot.tests.Unit.Shared;
//using Shouldly;

//namespace MySpot.tests.Unit.Services
//{
//    public class ReservationServiceTests
//    {
//        [Fact]
//        public async Task gieven_valid_command_create_should_add_reservation()
//        {
//            // ARRANGE
//            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"), 
//                Guid.NewGuid(), "Joe Doe", "XYZ123", _clock.Current().AddDays(1));

//            // ACT
//            var reservationId = await _reservationService.CreateAsync(command);

//            // ASSERT
//            reservationId.ShouldNotBeNull();
//            reservationId.Value.ShouldBe(command.ReservationId);
//        }

//        [Fact]
//        public async Task gieven_invalid_parking_spot_id_create_should_fail()
//        {
//            // ARRANGE
//            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000010"),
//                Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

//            // ACT
//            var reservationId = await _reservationService.CreateAsync(command);

//            // ASSERT
//            reservationId.ShouldBeNull();
//        }

//        [Fact]
//        public async Task given_reservation_for_already_taken_date_create_should_fail()
//        {
//            // ARRANGE
//            var command = new CreateReservation(Guid.Parse("00000000-0000-0000-0000-000000000001"),
//                Guid.NewGuid(), "Joe Doe", "XYZ123", DateTime.UtcNow.AddDays(1));

//            await _reservationService.CreateAsync(command);

//            // ACT
//            var reservationId = await _reservationService.CreateAsync(command);

//            // ASSERT
//            reservationId.ShouldBeNull();
//        }

//        #region ARRANGE

//        private readonly TestClock _clock;
//        private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
//        private readonly ReservationService _reservationService;

//        public ReservationServiceTests()
//        {
//            _clock = new TestClock ();
//            _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);

//            _reservationService = new ReservationService(_clock, _weeklyParkingSpotRepository);
//        }

//        #endregion
//    }
//}
