namespace DBModels.Dto;

public class PaymentDto
{
    public int Bookingid { get; set; }
    public decimal Amountpaid { get; set; }
    public string Paymentmethod { get; set; } = string.Empty;
    public int Userid { get; set; }
    public DateTime Paymentdate { get; set; }
}