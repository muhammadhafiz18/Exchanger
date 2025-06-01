using System.Text.Json;
using Exchanger.Interfaces;
using Exchanger.Models;

namespace Exchanger.Services;

public class ExchangeService(HttpClient httpClient) : IExchangeService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CurrencyResponse> GetCurrentCurrencyAsync(double amount, string baseSymbol, string rateSybmol)
    {
        var response = await _httpClient.GetAsync($"https://api.frankfurter.dev/v1/latest?base={baseSymbol}&symbols={rateSybmol}");

        if(!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrive data for {baseSymbol} and {rateSybmol}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var currencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(json, options);

        return currencyResponse;
    }

    public async Task<CurrencyResponse> GetHistoricalCurrencyAsync(int year, int month, int day)
    {
        var response = await _httpClient.GetAsync($"https://api.frankfurter.dev/v1/{year}-{month}-{day}?symbols=USD");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrive data for the date: {year}, {month}, {day}");
        }

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var currencyResponse = JsonSerializer.Deserialize<CurrencyResponse>(json, options);

        return currencyResponse;
    }
}
