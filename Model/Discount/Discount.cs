namespace s28201_Project.Model;

public class Discount
{
    public long DiscountId { get; set; }
    public string Desc { get; set; }
    public decimal ValuePercent { get; set; }
    public DistributionType DistributionType { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly ValidTill { get; set; }
    
    public virtual List<CompanyClient> CompanyClients { get; set; }
    public virtual List<IndividualClient> IndividualClients { get; set; }
}