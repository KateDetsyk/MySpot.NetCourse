using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("parking-spots")]
    public class ParkingSpotsController : ControllerBase
    {
        private readonly ICommandHandler<ReserveParkingSpotForVehicle> _reserveParkingSpotsForVehicleHandler;
        private readonly ICommandHandler<ReserveParkingSpotForCleaning> _reserveParkingSpotsForCleaningHandler;
        private readonly IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> 
            _getWeeklyParkingSpotsHandler;


        public ParkingSpotsController(ICommandHandler<ReserveParkingSpotForVehicle> reserveParkingSpotsForVehicleHandler,
            ICommandHandler<ReserveParkingSpotForCleaning> reserveParkingSpotsForCleanindHandler,
            IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsHandler)
        {
            _reserveParkingSpotsForVehicleHandler = reserveParkingSpotsForVehicleHandler;
            _reserveParkingSpotsForCleaningHandler = reserveParkingSpotsForCleanindHandler;
            _getWeeklyParkingSpotsHandler = getWeeklyParkingSpotsHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeeklyParkingSpotDto>>> Get([FromQuery] GetWeeklyParkingSpots query)
        {
            return Ok(await _getWeeklyParkingSpotsHandler.HandleAsync(query));
        }

        [HttpPost("{parkingSpotId:guid}/reservations/vehicle")]
        public async Task<ActionResult> Post(Guid parkingSpotId, ReserveParkingSpotForVehicle command)
        {
            await _reserveParkingSpotsForVehicleHandler.HandleAsync(command with { ParkingSpotId = parkingSpotId});
            return NoContent();
        }

        [HttpPost("reservations/cleaning")]
        public async Task<ActionResult> Post(Guid parkingSpotId, ReserveParkingSpotForCleaning command)
        {
            await _reserveParkingSpotsForCleaningHandler.HandleAsync(command);
            return NoContent();
        }
    }
}
