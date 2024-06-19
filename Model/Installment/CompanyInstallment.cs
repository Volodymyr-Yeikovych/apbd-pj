namespace s28201_Project.Model;

public class CompanyInstallment : Installment
{
    public long CompanyContractId { get; set; }
    public virtual CompanyContract CompanyContract { get; set; }
}