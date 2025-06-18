using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
        //  UserId from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized("User ID not found in token");

        int userId = int.Parse(userIdClaim.Value);

        // Call logic with UserId
        var success = await _logic.BookSeatAsync(dto, userId);
        if (!success)
            return BadRequest("Seat already booked or not found");

        return Ok("Seat booked successfully");
    }
}


