using System.Text.Json.Serialization;

namespace Exchanger.Models;

public class HistoricalResponse
{
    public string Base { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; }
    public Dictionary<string, Dictionary<string, double>> Rates { get; set; }

    // public override string ToString()
    // {
    //     // lyuboy ish
    //     return $"{StartDate} | {EndDate}";
    // }
}