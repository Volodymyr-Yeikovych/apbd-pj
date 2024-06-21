using s28201_Project.Context;
using s28201_Project.Dto;
using s28201_Project.Model;

namespace s28201_Project.Service.Installment;

public class CompanyInstallmentService(ApiContext context, CompanyContractService contractService)
{
    public async Task<InstallmentResponse> ProcessInstallmentAsync(CompanyInstallmentDto dto)
    {
        var response = new InstallmentResponse();

        if (dto.DistributionType != DistributionType.Upfront)
        {
            response.message = "Is not a contract installment.";
            return response;
        }

        var contract = await contractService.GetContractByIdAsync(dto.CompanyId);

        if (contract == null)
        {
            response.message = "Cannot pay for non-existent contract.";
            return response;
        }

        if (await IsInvalidPaymentDateAsync(dto.DatePaid, contract.StartDate, contract.EndDate))
        {
            response.message = "Invalid payment date for given contract.";
            await contractService.TryDissolveIfExpiredAsync(contract);
            return response;
        }

        if (dto.Price < 0)
        {
            response.message = "Installment cannot have negative price.";
            return response;
        }

        var desc = dto.Desc ?? "";
        var installment = await CreateInstallmentAsync(contract.ContractId, dto.Price, dto.DatePaid, desc);

        contract.CorporateInstallments.Add(installment);
        await context.SaveChangesAsync();

        var isSigned = await contractService.TrySignContractAsync(contract);

        var priceDiff = await contractService.CalcRemainingPriceToPayAsync(contract);
        response.message = $"Installment was added, but you need to pay {priceDiff} more to sign the contract.";

        if (isSigned)
        {
            response.message = "Contract was signed successfully.";
        }

        response.IsSuccessful = true;
        return response;
    }
    
    private async Task<CompanyInstallment> CreateInstallmentAsync(
        long contractContractId,
        decimal price,
        DateOnly datePaid,
        string desc)
    {
        var installment = new CompanyInstallment
        {
            CompanyContractId = contractContractId,
            Price = price,
            DatePaid = datePaid,
            DateConfirmed = DateOnly.FromDateTime(DateTime.Now),
            Desc = desc
        };

        await context.CorporateInstallments.AddAsync(installment);
        await context.SaveChangesAsync();

        return installment;
    }

    private Task<bool> IsInvalidPaymentDateAsync(DateOnly datePaid, DateOnly contractStartDate,
        DateOnly contractEndDate)
    {
        var isBetween = datePaid >= contractStartDate && datePaid <= contractEndDate;
        return Task.FromResult(isBetween);
    }
}