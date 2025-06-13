namespace DBModels.Dto
{
    public class ShowDto
    {
        public int ShowId { get; set; }
        public int MovieId { get; set; }
        public int TheaterId { get; set; }
        public decimal TicketPrice { get; set; }
    }
}