using System.Text.Json;
using Exchanger.Services;

var httpClient = new HttpClient();

var exchangeService = new ExchangeService(httpClient);

string[] currencySymbols = [
    "AUD", "BGN", "BRL", "CAD", "CHF", "CNY", "CZK", "DKK",
    "GBP", "HKD", "HUF", "IDR", "ILS", "INR", "ISK", "JPY",
    "KRW", "MXN", "MYR", "NOK", "NZD", "PHP", "PLN", "RON",
    "SEK", "SGD", "THB", "TRY", "USD", "ZAR", "EUR"
];

while (true)
{
    try
    {
        Console.Write("Available commands: Current. Historical, Historical2, Top: ");
        var option = Console.ReadLine()!.Trim().ToLower();

        if (option == "current")
        {
            Console.Write("Enter amount and symbol respectively: ");
            var input = Console.ReadLine()!.Trim().ToLower().Split(" "); // "5 usd"

            Console.Write("Now enter symbol that you want to exchange: ");
            var input2 = Console.ReadLine()!.Trim().ToLower();

            if (!currencySymbols.Contains(input[1].ToUpper()) || !currencySymbols.Contains(input2.ToUpper()))
            {
                throw new Exception($"Invalid symbol: {input[1]}");
            }

            var response = await exchangeService.GetCurrentCurrencyAsync(double.Parse(input[0]), input[1], input2);

            Console.WriteLine($"{response.Date} holatiga ko’ra — {input[0]} {input[1]} - {response.Rates[input2.ToUpper()] * double.Parse(input[0])} {input2}’ga teng");

            // response.PrintResult(double.Parse(input[0]), input2);
        }
        else if (option == "historical")
        {
            Console.Clear();
            Console.WriteLine("Enter year, month, and day respectively: ");
            int year = int.Parse(Console.ReadLine()!);
            int month = int.Parse(Console.ReadLine()!);
            int day = int.Parse(Console.ReadLine()!);

            var result = await exchangeService.GetHistoricalCurrencyAsync(year, month, day);

            // result.PrintResult(int.Parse(Console.ReadLine()), );
        }
        else if (option == "historical2")
        {
            Console.Clear();
            Console.WriteLine("Enter start year, month, and day respectively: ");
            int year = int.Parse(Console.ReadLine()!);
            int month = int.Parse(Console.ReadLine()!);
            int day = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter end year, month, and day respectively: ");
            int year2 = int.Parse(Console.ReadLine()!);
            int month2 = int.Parse(Console.ReadLine()!);
            int day2 = int.Parse(Console.ReadLine()!);

            var result = await exchangeService.GetDurationCurrencyAsync(year, month, day, year2, month2, day2);

            foreach (var kvp in result.Rates)
            {
                Console.WriteLine(kvp.Key); // "2000.10.10"
                foreach (var kvp2 in kvp.Value)
                {
                    Console.WriteLine($"    {kvp2.Key}: {kvp2.Value}");
                    //                   "USD":      1.22432343
                }
            }
        }

        else if (option == "top")
        {   
            if (File.Exists("topCurrencies.json") && new FileInfo("topCurrencies.json").Length > 0)
            {
                var json = File.ReadAllText("topCurrencies.json");
                var cachedTopCurrencies = JsonSerializer.Deserialize<Dictionary<string, double>>(json);
                
                Console.WriteLine("Retrieved from cache:");
                foreach (var item in cachedTopCurrencies)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
            }
            else
            {
                var response = await exchangeService.GetAllCurrentCurrencyAsync();
                var results = response.Rates.OrderByDescending(x => x.Value).Take(5).ToDictionary();
                
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(results, options);
                File.WriteAllText("topCurrencies.json", json);
                
                Console.WriteLine("Retrieved from API and cached:");
                foreach (var item in results)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
            }
        }

        else if (option == "exit")
        {
            Console.WriteLine("good bye👋");
            break;
        }
        else if (option == "cls")
        {
            Console.Clear();
        }
        else
        {
            Console.WriteLine("Invalid command.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Amount or symbol was not in a correct format!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
