namespace s28201_Project.Model;

public class CompanyContract : Contract
{
    public long CompanyId { get; set; }
    public virtual CompanyClient CompanyClient { get; set; }
    public virtual List<CompanyInstallment> CorporateInstallments { get; set; }
}