namespace Booking_Service.HttpClients
{
    public interface IPaymentServiceClient
    {
        Task<int> CreatePaymentAsync(int bookingId);
        Task<string?> ProcessPaymentAsync(int paymentId);
    }
}
