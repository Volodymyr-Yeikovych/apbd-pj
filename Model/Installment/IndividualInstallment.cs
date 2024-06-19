namespace s28201_Project.Model;

public class IndividualInstallment : Installment
{
    public long IndividualContractId { get; set; }
    public virtual IndividualContract IndividualContract { get; set; }
}