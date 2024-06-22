using Microsoft.EntityFrameworkCore;
using s28201_Project.Model;
using s28201_Project.Model.Employee;

namespace s28201_Project.Context;

public class ApiContext : DbContext
{
    public DbSet<CompanyClient> CompanyClients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<CompanyContract> CompanyContracts { get; set; }
    public DbSet<IndividualContract> IndividualContracts { get; set; }
    public DbSet<CompanyInstallment> CorporateInstallments { get; set; }
    public DbSet<IndividualInstallment> IndividualInstallments { get; set; }
    public DbSet<SoftwareLicense> SoftwareLicenses { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected ApiContext()
    {
    }

    public ApiContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost;Database=Project;User=sa;Password=fY0urP@sswor_Policy;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyClient>(e =>
        {
            e.HasKey(cc => cc.ClientId);
            e.Property(cc => cc.Name).IsRequired().HasMaxLength(100);
            e.Property(cc => cc.Address).IsRequired().HasMaxLength(100);
            e.Property(cc => cc.Email).IsRequired().HasMaxLength(100);
            e.Property(cc => cc.PhoneNum).IsRequired().HasMaxLength(100);
            e.Property(cc => cc.KrsNum).IsRequired().HasMaxLength(100);

            e.HasMany(c => c.Discounts)
                .WithMany(d => d.CompanyClients);

            e.HasData(new CompanyClient()
                {
                    ClientId = 1,
                    Address = "address",
                    Email = "fmail",
                    PhoneNum = "111-000-222",
                    Name = "Piotrek Inc",
                    KrsNum = "ss",
                }
            );
        });

        modelBuilder.Entity<IndividualClient>(e =>
        {
            e.HasKey(c => c.ClientId);
            e.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
            e.Property(c => c.LastName).IsRequired().HasMaxLength(100);
            e.Property(c => c.Address).IsRequired().HasMaxLength(100);
            e.Property(c => c.Email).IsRequired().HasMaxLength(100);
            e.Property(c => c.PhoneNum).IsRequired().HasMaxLength(100);
            e.Property(c => c.Pesel).IsRequired().HasMaxLength(100);

            e.HasMany(c => c.Discounts)
                .WithMany(d => d.IndividualClients);

            e.HasData(new IndividualClient()
                {
                    ClientId = 1,
                    Address = "address",
                    Email = "fmail",
                    PhoneNum = "111-000-222",
                    FirstName = "Piotrek",
                    LastName = "Szyb",
                    Pesel = "ss",
                }
            );
        });

        modelBuilder.Entity<Discount>(e =>
        {
            e.HasKey(c => c.DiscountId);
            e.Property(c => c.Desc).IsRequired().HasMaxLength(255);
            e.Property(c => c.ValuePercent).IsRequired();
            e.Property(c => c.DistributionType).IsRequired().HasMaxLength(100);
            e.Property(c => c.ValidFrom).IsRequired();
            e.Property(c => c.ValidTill).IsRequired();
        });

        modelBuilder.Entity<CompanyContract>(e =>
        {
            e.HasKey(c => c.ContractId);
            e.Property(c => c.TotalPrice).IsRequired();
            e.Property(c => c.StartDate).IsRequired();
            e.Property(c => c.EndDate).IsRequired();
            e.Property(c => c.AdditionalSupportYears).IsRequired();
            e.Property(c => c.IsSigned).IsRequired();
            e.Property(c => c.DiscountPercentage).IsRequired().HasDefaultValue(0);

            e.HasOne(c => c.Software)
                .WithMany(s => s.CompanyContracts)
                .HasForeignKey(c => c.SoftwareId);

            e.HasOne(c => c.CompanyClient)
                .WithMany(c => c.Contracts)
                .HasForeignKey(c => c.CompanyId);
        });

        modelBuilder.Entity<IndividualContract>(e =>
        {
            e.HasKey(c => c.ContractId);
            e.Property(c => c.TotalPrice).IsRequired();
            e.Property(c => c.StartDate).IsRequired();
            e.Property(c => c.EndDate).IsRequired();
            e.Property(c => c.AdditionalSupportYears).IsRequired();
            e.Property(c => c.IsSigned).IsRequired();
            e.Property(c => c.DiscountPercentage).IsRequired().HasDefaultValue(0);

            e.HasOne(c => c.Software)
                .WithMany(s => s.IndividualContracts)
                .HasForeignKey(c => c.SoftwareId);

            e.HasOne(c => c.IndividualClient)
                .WithMany(i => i.Contracts)
                .HasForeignKey(c => c.IndividualId);
        });

        modelBuilder.Entity<CompanyInstallment>(e =>
        {
            e.HasKey(c => c.InstallmentId);
            e.Property(c => c.Desc).IsRequired().HasMaxLength(255);
            e.Property(c => c.Price).IsRequired();
            e.Property(c => c.DatePaid).IsRequired();
            e.Property(c => c.DateConfirmed).IsRequired(false);

            e.HasOne(i => i.CompanyContract)
                .WithMany(c => c.CorporateInstallments)
                .HasForeignKey(i => i.CompanyContractId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<IndividualInstallment>(e =>
        {
            e.HasKey(c => c.InstallmentId);
            e.Property(c => c.Desc).IsRequired().HasMaxLength(255);
            e.Property(c => c.Price).IsRequired();
            e.Property(c => c.DatePaid).IsRequired();
            e.Property(c => c.DateConfirmed).IsRequired(false);

            e.HasOne(i => i.IndividualContract)
                .WithMany(c => c.IndividualInstallments)
                .HasForeignKey(i => i.IndividualContractId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<SoftwareLicense>(e =>
        {
            e.HasKey(c => c.SoftwareLicenseId);
            e.Property(c => c.Version).IsRequired().HasMaxLength(100);
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Desc).IsRequired().HasMaxLength(255);
            e.Property(c => c.StartingUpfrontPrice).IsRequired();
            e.Property(c => c.StartingMonthlySubPrice).IsRequired();
            e.Property(c => c.DistributionType).IsRequired().HasMaxLength(100);
            e.Property(c => c.LicenseCategory).IsRequired().HasMaxLength(100);

            e.HasData(
                new SoftwareLicense
                {
                    SoftwareLicenseId = 1,
                    Version = "1.0",
                    Name = "Basic License",
                    Desc = "This is a basic software license.",
                    StartingUpfrontPrice = 100.00m,
                    StartingMonthlySubPrice = 10.00m,
                    DistributionType = DistributionType.Subscription,
                    LicenseCategory = SoftwareLicenseCategory.Business
                },
                new SoftwareLicense
                {
                    SoftwareLicenseId = 2,
                    Version = "2.0",
                    Name = "Pro License",
                    Desc = "This is a professional software license.",
                    StartingUpfrontPrice = 200.00m,
                    StartingMonthlySubPrice = 20.00m,
                    DistributionType = DistributionType.Upfront,
                    LicenseCategory = SoftwareLicenseCategory.Productivity
                },
                new SoftwareLicense
                {
                    SoftwareLicenseId = 3,
                    Version = "3.0",
                    Name = "Enterprise License",
                    Desc = "This is an enterprise software license.",
                    StartingUpfrontPrice = 500.00m,
                    StartingMonthlySubPrice = 50.00m,
                    DistributionType = DistributionType.SubUpfront,
                    LicenseCategory = SoftwareLicenseCategory.Education
                }
            );
        });

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasKey(u => u.EmployeeId);
            e.Property(u => u.Login).IsRequired().HasMaxLength(100);
            e.Property(u => u.Password).IsRequired().HasMaxLength(100);
            e.Property(u => u.Role).IsRequired().HasMaxLength(100);

            e.HasData(
                new Employee
                {
                    EmployeeId = 1,
                    Login = "admin",
                    Password = "admin",
                    Role = Role.Admin
                },
                new Employee
                {
                    EmployeeId = 2,
                    Login = "user",
                    Password = "user",
                    Role = Role.User
                }
            );
        });
    }
}