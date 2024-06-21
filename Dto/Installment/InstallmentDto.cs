using s28201_Project.Model;

namespace s28201_Project.Dto;

public abstract class InstallmentDto
{
    public DistributionType DistributionType { get; set; }
    public decimal Price { get; set; }
    public DateOnly DatePaid { get; set; }
    public string? Desc { get; set; }
}