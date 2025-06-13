namespace DBModels.Db
{
    public class ShowInstance
    {
        public int ShowInstanceId { get; set; }
        public int ShowTemplateId { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal TicketPrice { get; set; }

        public ShowTemplate ShowTemplate { get; set; }
    }
}