using Microsoft.EntityFrameworkCore;
using s28201_Project.Model;
// using s28201_Project.Model.ClientDiscounts;

namespace s28201_Project.Context;

public class ApiContext : DbContext
{
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
    }
}