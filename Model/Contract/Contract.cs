namespace s28201_Project.Model.Contract;

public class Contract
{
    // TODO: Move to contract calculation price service?
    private static readonly long UPDATES_YEAR_PRICE = 1000;
    
    public long ContactId { get; set; }
    // = years of support + software price
    public decimal TotalPrice { get; set; }
    // 3-30 days
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    // 0-1-2-3
    public int AdditionalSupportYears { get; set; }
    public bool IsSigned { get; set; }
    
    public long SoftwareId { get; set; }
    public virtual SoftwareLicense Software { get; set; }
    public long DiscountId { get; set; }
    public virtual Discount Discount { get; set; }
    public long ClientId { get; set; }
    public virtual Client Client { get; set; }
}