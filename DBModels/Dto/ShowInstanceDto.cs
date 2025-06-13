namespace DBModels.Dto
{
    public class ShowInstanceDto
    {
        public int ShowInstanceId { get; set; }
        public int ShowTemplateId { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal TicketPrice { get; set; }
    }
}