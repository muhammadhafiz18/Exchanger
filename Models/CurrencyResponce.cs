namespace Exchanger.Models;

public class CurrencyResponse
{
    // public int Amount { get; set; }
    public string Base { get; set; }
    public string Date { get; set; }
    public Dictionary<string, double> Rates { get; set; }
}