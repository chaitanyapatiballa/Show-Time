namespace BookingService.DTOs;

public class BillingsummaryDto
{
    public int Bookingid { get; set; }
    public decimal Baseamount { get; set; }
    public decimal Discount { get; set; }
    public decimal Gst { get; set; }
    public decimal Servicefee { get; set; }
    public decimal Totalamount { get; set; }
}
