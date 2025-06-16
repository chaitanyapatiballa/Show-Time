using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class TheaterLogic   
{
    private readonly TheaterRepository _repository;
    private readonly AppDbContext _context;

    public TheaterLogic(AppDbContext context, TheaterRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<List<Theater>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Theater?> GetByIdAsync(int id) =>
        await _repository.GetByIdAsync(id);
    public async Task AddAsync(Theater theater, List<int>? movieIds)
    {
        if (movieIds != null && movieIds.Any())
        {
            var movies = await _context.Movies
                .Where(m => movieIds.Contains(m.Movieid))
                .ToListAsync();

            theater.MovieTheaters = movies.Select(m => new movietheater
            {
                movieid = m.Movieid,
                Theater = theater
            }).ToList();
        }

        _context.Theaters.Add(theater);
        await _context.SaveChangesAsync();
    }



    public async Task UpdateAsync(Theater theater)
    {
        var existing = await _context.Theaters.FindAsync(theater.Theaterid);
        if (existing == null) return;

        existing.Name = theater.Name;
        existing.Location = theater.Location;

        _context.Theaters.Update(existing);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(Theater theater) =>
        await _repository.DeleteAsync(theater);
}
