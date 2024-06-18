namespace s28201_Project.Model;

public class PermanentPurchase : AbstractPurchase
{
    public long PermanentPurchaseId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNum { get; set; }
    
    // TODO: Not Implemented
    public override decimal GetTotalPredictedPaidPln()
    {
        throw new NotImplementedException();
    }
}