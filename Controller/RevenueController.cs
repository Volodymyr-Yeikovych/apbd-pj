using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s28201_Project.Service.Revenue;

namespace s28201_Project.Controller;

[ApiController]
[Route("api/revenue/")]
public class RevenueController(RevenueService service, ICurrencyService currencyService) : ControllerBase
{
    
    [HttpGet("{currencyCode}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetRevenueCompanyAsync(string currencyCode = "PLN")
    {
        var revenue = await service.GetTotalRevenueAsync();

        var revenueInCurrencyResponse = await currencyService.FromPlnAsync(revenue, currencyCode);

        if (revenueInCurrencyResponse == null)
        {
            return BadRequest($"Invalid currency code[{currencyCode}]");
        }

        return Ok(revenueInCurrencyResponse);
    }
    
    [HttpGet("predicted/{currencyCode}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetPredictedRevenueCompanyAsync(string currencyCode = "PLN")
    {
        var predictedRevenue = await service.GetPredictedRevenueAsync();
        
        var revenueInCurrencyResponse = await currencyService.FromPlnAsync(predictedRevenue, currencyCode);
        
        if (revenueInCurrencyResponse == null)
        {
            return BadRequest($"Invalid currency code[{currencyCode}]");
        }
        
        return Ok(revenueInCurrencyResponse);
    }
    
    [HttpGet("{id:long}/{currencyCode}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetRevenueProduct(long id, string currencyCode = "PLN")
    {
        var productRevenue = await service.GetProductRevenueAsync(id);
        
        if (productRevenue == null) return BadRequest($"Product with id[{id}] does not exists.");
        
        var revenueInCurrencyResponse = await currencyService.FromPlnAsync((decimal) productRevenue, currencyCode);
        
        if (revenueInCurrencyResponse == null)
        {
            return BadRequest($"Invalid currency code[{currencyCode}]");
        }
        
        return Ok(revenueInCurrencyResponse);
    }
    
    
    [HttpGet("predicted/{id:long}/{currencyCode}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetPredictedRevenueProduct(long id, string currencyCode = "PLN")
    {
        var productPredictedRevenue = await service.GetProductPredictedRevenueAsync(id);
        
        if (productPredictedRevenue == null) return BadRequest($"Product with id[{id}] does not exists.");
        
        var revenueInCurrencyResponse = await currencyService.FromPlnAsync((decimal) productPredictedRevenue, currencyCode);
        
        if (revenueInCurrencyResponse == null)
        {
            return BadRequest($"Invalid currency code[{currencyCode}]");
        }
        
        return Ok(revenueInCurrencyResponse);
    }
}