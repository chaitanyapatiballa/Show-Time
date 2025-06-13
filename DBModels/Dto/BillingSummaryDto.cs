namespace DBModels.Dto
{
    public class BillingSummaryDto
    {
        public int BookingId { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal GST { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal FinalAmount { get; set; }
    }
}