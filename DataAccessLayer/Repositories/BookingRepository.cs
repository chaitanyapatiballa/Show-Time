using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;
    
public class BookingRepository(AppDbContext context)
{
    private readonly AppDbContext _context = context;

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
