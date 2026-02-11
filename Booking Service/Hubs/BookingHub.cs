using DBModels.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Hubs;

public class BookingHub : Hub
{
    private readonly AppDbContext _context;

    public BookingHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task JoinShowGroup(int showInstanceId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, showInstanceId.ToString());
    }

    public async Task LockSeat(int showInstanceId, int seatId)
    {
        var seatStatus = await _context.Showseatstatuses
            .FirstOrDefaultAsync(s => s.Showinstanceid == showInstanceId && s.Seatid == seatId);

        if (seatStatus == null)
        {
            await Clients.Caller.SendAsync("Error", "Seat not found");
            return;
        }

        if (seatStatus.Isbooked)
        {
            await Clients.Caller.SendAsync("Error", "Seat already booked");
            return;
        }

        // Check if locked and not expired (e.g., 10 mins)
        if (seatStatus.LockedAt.HasValue && seatStatus.LockedAt.Value.AddMinutes(10) > DateTime.UtcNow)
        {
            var locker = Context.UserIdentifier ?? Context.ConnectionId;
            if (seatStatus.LockedBy != locker)
            {
                await Clients.Caller.SendAsync("Error", "Seat is currently locked by another user");
                return;
            }
        }

        // Lock the seat
        seatStatus.LockedAt = DateTime.UtcNow;
        seatStatus.LockedBy = Context.UserIdentifier ?? Context.ConnectionId; // Prefer UserId
        await _context.SaveChangesAsync();

        await Clients.Group(showInstanceId.ToString()).SendAsync("SeatLocked", seatId, Context.ConnectionId);
    }

    public async Task ReleaseSeat(int showInstanceId, int seatId)
    {
        var seatStatus = await _context.Showseatstatuses
            .FirstOrDefaultAsync(s => s.Showinstanceid == showInstanceId && s.Seatid == seatId);

        if (seatStatus != null && seatStatus.LockedBy == Context.ConnectionId)
        {
            seatStatus.LockedAt = null;
            seatStatus.LockedBy = null;
            await _context.SaveChangesAsync();

            await Clients.Group(showInstanceId.ToString()).SendAsync("SeatReleased", seatId);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Optional: Release all seats locked by this connection on disconnect
        // For production, this should be done with care or a background job cleanup
        await base.OnDisconnectedAsync(exception);
    }
}
