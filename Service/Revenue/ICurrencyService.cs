namespace s28201_Project.Service.Revenue;

public interface ICurrencyService
{
    public Task<decimal?> FromPlnAsync(decimal revenue, string currencyCode);
}