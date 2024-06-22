using Microsoft.EntityFrameworkCore;
using s28201_Project.Context;
using s28201_Project.Model;

namespace s28201_Project.Service.Revenue;

public class RevenueService(ApiContext context)
{
    public async Task<decimal> GetTotalRevenueAsync()
    {
        var revenueCompanies = await context.CompanyContracts
            .Where(c => c.IsSigned)
            .SumAsync(c => c.TotalPrice);
        
        var revenueIndividuals = await context.IndividualContracts
            .Where(c => c.IsSigned)
            .SumAsync(c => c.TotalPrice);

        var total = revenueCompanies + revenueIndividuals;

        return total;
    }

    public async Task<decimal> GetPredictedRevenueAsync()
    {
        var revenueCompanies = await context.CompanyContracts
            .SumAsync(c => c.TotalPrice);
        
        var revenueIndividuals = await context.IndividualContracts
            .SumAsync(c => c.TotalPrice);

        var totalPredicted = revenueCompanies + revenueIndividuals;

        return totalPredicted;
    }

    public async Task<decimal?> GetProductRevenueAsync(long id)
    {
        var revenueCompanies = await context.CompanyContracts
            .Include(c => c.Software)
            .Where(c => c.IsSigned && c.SoftwareId == id)
            .SumAsync(c => c.TotalPrice);
        
        var revenueIndividuals = await context.IndividualContracts
            .Include(c => c.Software)
            .Where(c => c.IsSigned && c.SoftwareId == id)
            .SumAsync(c => c.TotalPrice);

        var total = revenueCompanies + revenueIndividuals;

        return total;
    }

    public async Task<decimal?> GetProductPredictedRevenueAsync(long id)
    {
        var revenueCompanies = await context.CompanyContracts
            .Include(c => c.Software)
            .Where(c => c.SoftwareId == id)
            .SumAsync(c => c.TotalPrice);
        
        var revenueIndividuals = await context.IndividualContracts
            .Include(c => c.Software)
            .Where(c => c.SoftwareId == id)
            .SumAsync(c => c.TotalPrice);

        var totalPredicted = revenueCompanies + revenueIndividuals;

        return totalPredicted;
    }
}