using Exchanger.Services;


var httpClient = new HttpClient();

var exchangeService = new ExchangeService(httpClient);

string[] currencySymbols = [
    "AUD", "BGN", "BRL", "CAD", "CHF", "CNY", "CZK", "DKK",
    "GBP", "HKD", "HUF", "IDR", "ILS", "INR", "ISK", "JPY",
    "KRW", "MXN", "MYR", "NOK", "NZD", "PHP", "PLN", "RON",
    "SEK", "SGD", "THB", "TRY", "USD", "ZAR"
];

while (true)
{
    try
    {
        Console.Write("Available commands: Current. Historical: ");
        var option = Console.ReadLine()!.Trim().ToLower();

        if (option == "current")
        {
            Console.Write("Enter amount and symbol respectively: ");
            var input = Console.ReadLine()!.Trim().ToLower().Split(" "); // "5 usd"

            Console.Write("Now enter symbol that you want to exchange: ");
            var input2 = Console.ReadLine()!.Trim().ToLower();

            if (!currencySymbols.Contains(input[0]) || !currencySymbols.Contains(input2))
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