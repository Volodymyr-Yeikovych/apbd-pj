namespace s28201_Project.Model;

public class IndividualContract : Contract
{
    public long IndividualId { get; set; }
    public virtual IndividualClient IndividualClient { get; set; }
    public virtual List<IndividualInstallment> IndividualInstallments { get; set; }
}