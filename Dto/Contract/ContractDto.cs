
namespace s28201_Project.Dto;

public class ContractDto
{
    public string ClientIdentifier { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int AdditionalSupportYears { get; set; }

    public SoftwareLicenseDto License { get; set; }
}