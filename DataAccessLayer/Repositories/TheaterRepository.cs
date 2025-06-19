using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TheaterRepository
{
    private readonly AppDbContext _context;

    public TheaterRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Theater>> GetAllAsync() => await _context.Theaters.ToListAsync();

    public async Task<Theater?> GetByIdAsync(int id) => await _context.Theaters.FindAsync(id);

    public async Task AddAsync(Theater theater)
    {
        _context.Theaters.Add(theater);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Theater theater)
    {
        _context.Theaters.Update(theater);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Theater theater)
    {
        _context.Theaters.Remove(theater);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() => await _context.Showtemplates.ToListAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) => await _context.Showtemplates.FindAsync(id);

    public async Task AddShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Add(template);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShowtemplateAsync(Showtemplate template)
    {
        _context.Showtemplates.Remove(template);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Showinstance>> GetAllShowinstancesAsync() => await _context.Showinstances.ToListAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) => await _context.Showinstances.FindAsync(id);

    public async Task AddShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Add(instance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Update(instance);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShowinstanceAsync(Showinstance instance)
    {
        _context.Showinstances.Remove(instance);
        await _context.SaveChangesAsync();
    }
}
