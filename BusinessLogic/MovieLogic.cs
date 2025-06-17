using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class MovieLogic
{
    private readonly AppDbContext _context;
    private readonly MovieRepository _repository;

    public MovieLogic(AppDbContext context)
    {
        _context = context;
        _repository = new MovieRepository(context);
    }

    // Movie
    public async Task<List<Movie>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Movie?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Movie> AddAsync(Movie movie)
    {
        return await _repository.AddMovieAsync(movie);
    }

    public async Task UpdateAsync(int id, MovieDto dto)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return;

        movie.Title = dto.Title;
        movie.Duration = dto.Duration;
        movie.Genre = dto.Genre;
        movie.releasedate = dto.releasedate;

        await _repository.UpdateAsync(movie);
    }
    public async Task DeleteAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie != null)
        {
            await _repository.DeleteAsync(movie);
        }
    }  

}
