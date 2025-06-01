using Exchanger.Models;

namespace Exchanger.Interfaces;

public interface IExchangeService
{
    Task<CurrencyResponse> GetHistoricalCurrencyAsync(int year, int month, int day);
    Task<CurrencyResponse> GetCurrentCurrencyAsync(double amount, string baseSymbol, string rateSybmol);
}