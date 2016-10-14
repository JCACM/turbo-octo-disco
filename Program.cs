using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public struct Day
        {
            public string symbol;
            public string date;
            public double open;
            public double high;
            public double low;
            public double close;
            public int volume;
            public double adjClose;

            public Day(string symbol, string line)
            {
                var day = line.Split(',');

                this.symbol = symbol;
                this.date = day[0];
                this.open = double.Parse(day[1]);
                this.high = double.Parse(day[2]);
                this.low = double.Parse(day[3]);
                this.close = double.Parse(day[4]);
                this.volume = int.Parse(day[5]);
                this.adjClose = double.Parse(day[6]);
            }
        }

        public static void Main(string[] args)
        {
            ProcessStockSymbols();
            Console.WriteLine("Gathering Yahoo Data...");
            Console.ReadLine();
        }

        static async void ProcessStockSymbols()
        {
            var symbols = new string[] {
                "SPY",
                "QQQ",
                "TSLA",
                "NFLX",
                "AMZN",
                "NKE",
                "AAPL",
                "GOOG",
                "MSFT"
            };

            var tasks = symbols.Select(symbol => ProcessStockSymbol(symbol)).ToList();
            var results = await Task.WhenAll(tasks);
            var dayWinners = results.Where(stockResult => stockResult.Item2 == true).Select(stockResult => stockResult.Item1).ToList();
            var weekWinners = results.Where(stockResult => stockResult.Item3 == true).Select(stockResult => stockResult.Item1).ToList();
            var monthWinners = results.Where(stockResult => stockResult.Item4 == true).Select(stockResult => stockResult.Item1).ToList();

            Console.WriteLine($"Day Winners: {string.Join(" ", dayWinners)}");
            Console.WriteLine($"Week Winners: {string.Join(" ", weekWinners)}");
            Console.WriteLine($"Month Winners: {string.Join(" ", monthWinners)}");
        }

        static async Task<Tuple<string, bool, bool, bool>> ProcessStockSymbol(string symbol)
        {
            var url = $"http://chart.finance.yahoo.com/table.csv?s={symbol}&a=9&b=11&c=2015&d=9&e=13&f=2016&g=d&ignore=.csv";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                Console.WriteLine($"Fetching {symbol}...");
                string result = await content.ReadAsStringAsync();

                var lines = result.Split('\n').Skip(1);

                var days = lines
                            .Where(line => line != "")
                            .Select(line => new Day(symbol, line))
                            .ToList();

                return Tuple.Create(
                    symbol,
                    MatchesDayPattern(days),
                    MatchesWeekPattern(days),
                    MatchesMonthPattern(days)
                );
            }
        }

        static bool MatchesDayPattern(List<Day> days)
        {
            return (days[0].close > days[0].open && days[5].close > days[5].open);
        }

        static bool MatchesWeekPattern(List<Day> days)
        {
            return (days[0].close > days[0].open && days[7].close > days[5].open);
        }

        static bool MatchesMonthPattern(List<Day> days)
        {
            return (days[2].close > days[0].open && days[5].close > days[5].open);
        }
    }
}
