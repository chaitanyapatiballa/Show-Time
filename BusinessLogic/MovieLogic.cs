using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic;

public class MovieLogic
{
    private readonly MovieRepository _repository;

    public MovieLogic(MovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Movie>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Movie?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Movie> AddAsync(Movie movie) => await _repository.AddMovieAsync(movie);

    public async Task UpdateAsync(int id, MovieDto dto)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return;

        movie.Title = dto.Title;
        movie.Duration = dto.Duration;
        movie.Genre = dto.Genre;
        movie.Releasedate = dto.Releasedate;    

        await _repository.UpdateAsync(movie);
    }

    public async Task DeleteAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie != null)
            await _repository.DeleteAsync(movie);
    }

    public async Task<List<Showinstance>> GetShowinstancesForMovieAsync(int movieId)
        => await _repository.GetShowinstancesForMovieAsync(movieId);

    public async Task<List<Theater>> GetTheatersForMovieAsync(int movieId)
        => await _repository.GetTheatersForMovieAsync(movieId);
}

