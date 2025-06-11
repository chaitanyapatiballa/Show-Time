namespace DBModels.Db
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public string SeatNumber { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public bool IsCancelled { get; set; }
        public string Status { get; set; } = "Pending";
        public int PaymentId { get; set; }
        public Movie? Movie { get; set; }
        public Theater? Theater { get; set; }
        public Payment? Payment { get; set; }
    }
}
