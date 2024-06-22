using System.Text.Json.Serialization;

namespace s28201_Project.Service.Revenue;

public class NbpCurrencyService(HttpClient _httpClient) : ICurrencyService
{
    
    private readonly string API_REQUEST_URI = "http://api.nbp.pl/api/exchangerates/rates/A/";
    
    public async Task<decimal?> FromPlnAsync(decimal revenue, string currencyCode)
    {
        if (currencyCode.ToUpper() == "PLN") return revenue;

        string requestUri = API_REQUEST_URI + currencyCode;
        
        try
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var conversionResult = await response.Content.ReadFromJsonAsync<Root>();
            
            var rate = conversionResult?.Rates[0].Mid ?? 1;
            
            return revenue / rate;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return null;
        }
    }

    private class Root
    {
        [JsonPropertyName("table")]
        public string Table { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("rates")]
        public List<Rate> Rates { get; set; }
    }
    
    private class Rate
    {
        [JsonPropertyName("no")]
        public string No { get; set; }

        [JsonPropertyName("effectiveDate")]
        public string EffectiveDate { get; set; }

        [JsonPropertyName("mid")]
        public decimal Mid { get; set; }
    }
}