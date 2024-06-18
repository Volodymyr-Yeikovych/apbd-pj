namespace s28201_Project.Model;

public class SoftwareLicense
{
    public long SoftwareLicenseId { get; set; }
    public string Version { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public DistributionType DistributionType { get; set; }
    public SoftwareLicenseCategory LicenseCategory { get; set; }
}