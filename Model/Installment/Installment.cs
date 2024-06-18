namespace s28201_Project.Model;

public class Installment
{
    public long InstallmentId { get; set; }
    public decimal Price { get; set; }
    public DateOnly DatePaid { get; set; }
    public DateOnly? DateConfirmed { get; set; }
    public string? Desc { get; set; }

    public void Confirm()
    {
        DateConfirmed = DateOnly.FromDateTime(DateTime.Now);
    }
}