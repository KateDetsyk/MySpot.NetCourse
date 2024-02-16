﻿using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<ActionResult<ReservationDto[]>> Get()
    {
        var result = (await _reservationService.GetAllWeeklyAsync());
        return Ok(await _reservationService.GetAllWeeklyAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> Get(Guid id)
    {
        var reservation = await _reservationService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return reservation;
    }

    [HttpPost("vehicle")]
    public async Task<ActionResult> Post(ReserveParkingSpotForVehicle command)
    {
        await _reservationService.ReserveForVehicleAsync(command with { ReservationId = Guid.NewGuid() });

        return CreatedAtAction(nameof(Get), new {Id = command.ReservationId}, default);
    }

    [HttpPost("cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaning command)
    {
        await _reservationService.ReserveForVehicleCleaningAsync(command);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, ChangeReservationLicencePlate command)
    {
        await _reservationService.ChangeReservationLicencePlateAsync(command with { ReservationId = id });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _reservationService.DeleteAsync(new DeleteReservation(id));

        return NoContent();
    }
}
