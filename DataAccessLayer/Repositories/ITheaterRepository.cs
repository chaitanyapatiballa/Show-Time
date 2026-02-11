using DBModels.Models;

namespace DataAccessLayer.Repositories;

public interface ITheaterRepository
{
    // Theater Operations
    Task<List<Theater>> GetAllAsync();
    Task<Theater?> GetByIdAsync(int id);
    Task AddAsync(Theater theater);
    Task UpdateAsync(Theater theater);
    Task DeleteAsync(Theater theater);

    // Movie-Theater Links
    Task UpdateMovieLinksAsync(int theaterId, List<int> movieIds);

    // Seat Operations
    Task AddSeatsAsync(IEnumerable<Seat> seats);

    // ShowTemplate Operations
    Task<List<Showtemplate>> GetAllShowtemplatesAsync();
    Task<Showtemplate?> GetShowtemplateByIdAsync(int id);
    Task<Showtemplate> AddShowtemplateAsync(Showtemplate template);
    Task UpdateShowtemplateAsync(Showtemplate template);
    Task DeleteShowtemplateAsync(Showtemplate template);
    
    // ShowInstance Operations
    Task<List<Showinstance>> GetAllShowinstancesAsync();
    Task<Showinstance?> GetShowinstanceByIdAsync(int id);
    Task AddShowinstanceAsync(Showinstance instance);
    Task UpdateShowinstanceAsync(Showinstance instance);
    Task DeleteShowinstanceAsync(Showinstance instance);
    Task<List<Showinstance>> GetShowinstancesByMovieAsync(int movieId);
    Task<List<Showinstance>> GetShowinstancesByMovieTheaterAndDateAsync(int movieId, int theaterId, DateOnly date);
    
    // Seat Status Operations
    Task<List<Showseatstatus>> GetSeatStatusesByShowInstanceIdAsync(int showInstanceId);
    Task AddShowseatstatusesAsync(IEnumerable<Showseatstatus> statuses);
    Task RemoveShowseatstatusesAsync(IEnumerable<Showseatstatus> statuses);

    // Helper / Validation
    Task<bool> MovieTheaterLinkExistsAsync(int movieId, int theaterId);
    Task AddMovieTheaterLinkAsync(int movieId, int theaterId);
    Task<List<Seat>> GetSeatsByTheaterIdAsync(int theaterId);
    Task<Showtemplate?> GetShowtemplateWithTheaterAsync(int id);
}
