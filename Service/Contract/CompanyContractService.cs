﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<CompanyContract?> GetContractByIdAsync(long id)
    {
        return await context.CompanyContracts
            .Include(c => c.CompanyClient)
            .Include(c => c.CorporateInstallments)
            .FirstOrDefaultAsync(c => c.ContractId == id);
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

        await CreateAndSaveContractAsync(dto.StartDate, dto.EndDate, dto.AdditionalSupportYears, license, discount,
            client);

        response.IsSuccessful = true;
        return response;
    }
    
    public async Task TryDissolveIfExpiredAsync(CompanyContract contract)
    {
        if (await IsExpiredContractAsync(contract))
        {
            var toBePaid = await CalcRemainingPriceToPayAsync(contract);
            var toBeReturned = toBePaid * -1;
            if (toBeReturned > 0)
            {
                await ReturnExcessiveMoneyToClientAsync(contract, toBeReturned);
            }
            await DissolveContractAsync(contract);
        }
    }

    public Task<decimal> CalcRemainingPriceToPayAsync(CompanyContract contract)
    {
        var installments = contract.CorporateInstallments;
        var remainingPrice = contract.TotalPrice;

        foreach (var i in installments)
        {
            remainingPrice -= i.Price;
        }

        return Task.FromResult(remainingPrice);
    }

    public async Task<bool> TrySignContractAsync(long id)
    {
        var contract = await GetContractByIdAsync(id);
        if (contract == null) return false;
        return await TrySignContractAsync(contract);
    }

    public async Task<bool> TrySignContractAsync(CompanyContract contract)
    {
        var toBePaid = await CalcRemainingPriceToPayAsync(contract);
        switch (toBePaid)
        {
            case > 0:
                return false;
            case 0:
                return true;
            default:
                var toBeReturned = toBePaid * -1;
                await ReturnExcessiveMoneyToClientAsync(contract, toBeReturned);
                return true;
        }
    }

    private Task ReturnExcessiveMoneyToClientAsync(CompanyContract contract, decimal toBeReturned)
    {
        Console.WriteLine($"Amount: [{toBeReturned}] needs to be returned to client with KRS [{contract.CompanyClient.KrsNum}]");
        return Task.CompletedTask;
    }
    
    private async Task DissolveContractAsync(CompanyContract contract)
    {
        context.CompanyContracts.Remove(contract);
        await context.SaveChangesAsync();
    }

    private Task<bool> IsExpiredContractAsync(CompanyContract contract)
    {
        return Task.FromResult(contract.EndDate < DateOnly.FromDateTime(DateTime.Now));
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
    
    private Task<decimal> CalcTotalPriceAsync(int supportYears, decimal licensePrice, decimal discount)
    {
        var totalYearsSupport = 1 + supportYears;
        var result = licensePrice + totalYearsSupport * UpdatesYearPrice;
        var priceDiscountMultiplicator = 1 - discount / 100;
        result *= priceDiscountMultiplicator;

        return Task.FromResult(result);
    }

    private async Task<bool> IsInvalidProductSelectionAsync(SoftwareLicense license, CompanyClient client)
    {
        var licenses = await GetAllProductsForAsync(client);
        if (licenses.Count == 0) return false;

        if (licenses.Any(c => c.SoftwareLicenseId == license.SoftwareLicenseId)) return true;

        return false;
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
            TotalPrice = await CalcTotalPriceAsync(supportYears, license.StartingUpfrontPrice, discount),
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
}