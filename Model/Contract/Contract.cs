using System.ComponentModel.DataAnnotations.Schema;

namespace s28201_Project.Model;

public abstract class Contract
{
    public long ContractId { get; set; }
    // = years of support + software price
    [Column(TypeName = "decimal(36, 12)")]
    public decimal TotalPrice { get; set; }
    // 3-30 days
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    // 0-1-2-3
    public int AdditionalSupportYears { get; set; }
    public bool IsSigned { get; set; }
    [Column(TypeName = "decimal(9, 4)")]
    public decimal DiscountPercentage { get; set; }
    
    public long SoftwareId { get; set; }
    public virtual SoftwareLicense Software { get; set; }
}