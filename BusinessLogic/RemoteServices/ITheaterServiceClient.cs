using DBModels.Db;
using System.Threading.Tasks;

namespace Booking_Service.HttpClients
{
    public interface ITheaterServiceClient
    {
        Task<Theater> GetTheaterAsync(int theaterId);
    }
}
