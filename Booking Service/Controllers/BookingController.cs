using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly BookingLogic _logic;

    public BookingController(BookingLogic logic)
    {
        _logic = logic;
    }

    [HttpPost("bookseat")]
    public async Task<IActionResult> BookSeat([FromBody] BookingDto dto)
    {
        var success = await _logic.BookSeatAsync(dto);
        if (!success)
            return BadRequest("Seat already booked or not found");

        return Ok("Seat booked successfully");
    }
}


