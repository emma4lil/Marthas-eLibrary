using eLibrary.Application.Services;
using eLibrary.Domain.dtos.bookservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLibrary.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>
    /// Reserve a book
    /// </summary>
    /// <param name="reserveBookRequest"></param>
    /// <returns></returns>
    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveBook([FromBody] ReserveBookRequest reserveBookRequest)
    {
        var result = await _reservationService.ReserveBook(reserveBookRequest);
        return Ok(result.message);
    }

    [HttpGet("reservations")]
    [Authorize]
    public async Task<IActionResult> GetReservations()
    {
        var result = await _reservationService.GetReservations();
        return Ok(result.reservations);
    }
}