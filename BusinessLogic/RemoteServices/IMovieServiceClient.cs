using DBModels.Db;
using System.Threading.Tasks;

namespace Booking_Service.HttpClients
{
    public interface IMovieServiceClient
    {
        Task<Movie> GetMovieAsync(int movieId);
    }
}
