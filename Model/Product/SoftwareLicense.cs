using System.ComponentModel.DataAnnotations.Schema;

namespace s28201_Project.Model;

public class SoftwareLicense : IProduct
{
    public long SoftwareLicenseId { get; set; }
    public string Version { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    [Column(TypeName = "decimal(36, 12)")]
    public decimal StartingUpfrontPrice { get; set; }
    [Column(TypeName = "decimal(36, 12)")]
    public decimal StartingMonthlySubPrice { get; set; }
    public DistributionType DistributionType { get; set; }
    public SoftwareLicenseCategory LicenseCategory { get; set; }

    public virtual List<CompanyContract> CompanyContracts { get; set; }
    public virtual List<IndividualContract> IndividualContracts { get; set; }
}