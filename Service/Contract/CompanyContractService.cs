using Microsoft.EntityFrameworkCore;
using s28201_Project.Context;
using s28201_Project.Controller;
using s28201_Project.Dto;
using s28201_Project.Model;

namespace s28201_Project.Service;

public class CompanyContractService(
    ApiContext context, 
    CompanyClientService clientService,
    LicenseService licenseService
    )
{
    private const long UpdatesYearPrice = 1000;

    public async Task<Contract?> GetContractByIdAsync(long id)
    {
        return await context.CompanyContracts.FirstOrDefaultAsync(c => c.ContractId == id);
    }

    public async Task<ContractResponse> AddContractAsync(ContractDto dto)
    {
        var response = new ContractResponse();

        if (await IsInvalidStartEndTimeAsync(dto.StartDate, dto.EndDate))
        {
            response.Message = "Invalid start or end date.";
            return response;
        }

        if (await IsInvalidAdditionalSupportTimeAsync(dto.AdditionalSupportYears))
        {
            response.Message = "Invalid number of support years.";
            return response;
        }

        var client = await clientService.GetClientByKrsAsync(dto.ClientIdentifier);

        if (client == null)
        {
            response.Message = "Invalid client identifier.";
            return response;
        }

        var license = await licenseService.GetLicenseByIdAsync(dto.License.LicenseId);

        if (license == null)
        {
            response.Message = "Cannot buy non-existing license.";
            return response;
        }

        if (await IsInvalidProductSelectionAsync(license, client))
        {
            response.Message = "Invalid product selection.";
            return response;
        }

        var discount = await clientService.GetMaximalDiscountAsync(client);
        
        await CreateAndSaveContractAsync(dto.StartDate, dto.EndDate, dto.AdditionalSupportYears, license, discount, client);

        response.IsSuccess = true;
        return response;
    }

    private async Task CreateAndSaveContractAsync(
        DateOnly start,
        DateOnly end,
        int supportYears,
        SoftwareLicense license,
        decimal discount,
        CompanyClient client
    )
    {
        var contract = new CompanyContract
        {
            TotalPrice = await CalcTotalPriceAsync(supportYears, license.StartingUpfrontPrice),
            StartDate = start,
            EndDate = end,
            AdditionalSupportYears = supportYears,
            Software = license,
            DiscountPercentage = discount,
            CompanyClient = client
        };

        await context.CompanyContracts.AddAsync(contract);
        await context.SaveChangesAsync();
    }

    private Task<decimal> CalcTotalPriceAsync(int supportYears, decimal licensePrice)
    {
        var totalYearsSupport = 1 + supportYears;
        var result = licensePrice + totalYearsSupport * UpdatesYearPrice;
        
        return Task.FromResult(result);
    }

    private async Task<bool> IsInvalidProductSelectionAsync(SoftwareLicense license, CompanyClient client)
    {
        var licenses = await GetAllProductsForAsync(client);
        if (licenses.Count == 0) return false;
        
        if (licenses.Any(c => c.SoftwareLicenseId == license.SoftwareLicenseId)) return true;

        return false;
    }

    private async Task<List<SoftwareLicense>> GetAllProductsForAsync(CompanyClient client)
    {
        return await context.CompanyContracts
            .Include(c => c.Software)
            .Where(c => c.CompanyId == client.ClientId)
            .Select(c => c.Software)
            .ToListAsync();
    }

    private Task<bool> IsInvalidAdditionalSupportTimeAsync(int supportYears)
    {
        return Task.FromResult(supportYears is < 0 or > 3);
    }

    private Task<bool> IsInvalidStartEndTimeAsync(DateOnly st, DateOnly ed)
    {
        var start = st.ToDateTime(TimeOnly.MinValue);
        var end = ed.ToDateTime(TimeOnly.MinValue);

        if (start > end) return Task.FromResult(false);

        long daysDiff = (end - start).Days;

        return Task.FromResult(daysDiff is < 3 or > 30);
    }
}