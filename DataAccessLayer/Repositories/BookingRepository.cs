using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class BookingRepository
{
    private readonly AppDbContext _context;
    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> AddBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.ToListAsync();
    }
}
