using System;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

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
                "GOOG", "AAPL", "GOOGL", "MSFT", "AMZN", "FB", "XOM", "JNJ", "BABA", "GE", "CHL", "T", "JPM", "PG", "WMT", "VZ", "PFE", "BUD", "V", "CVX", "KO", "INTC", "MRK", "CMCSA", "ORCL", "HD", "PM", "CSCO", "BAC", "PEP", "TSM", "HSBC", "IBM", "DIS", "UN", "UL", "C", "UNH", "MO", "MDT", "MA", "MMM", "KHC", "SLB", "BP", "ABBV", "GSK", "SNY", "CVS", "ABEV", "QCOM", "BMY", "RY", "AGN", "NKE", "LLY", "HON", "WBA", "UTX", "CELG", "AZN", "BA", "TD", "UNP", "SBUX", "VOD", "ACN", "CHTR", "USB", "PCLN", "DEO", "BIDU", "RAI", "TXN", "GS", "SAN", "AVGO", "MDLZ", "COST", "BNS", "CL", "ITUB", "LOW", "AIG", "ABT", "MS", "BLK", "RIO", "TWX", "AXP", "CB", "DOW", "EPD", "DD", "ADBE", "HMC", "TEF", "COP", "CRM", "TEVA", "GM", "CNI", "TJX", "ING", "CAT", "MET", "HDB", "PYPL", "FDX", "F", "GD", "FOX", "LVS", "FOXA", "SYK", "PNC", "ESRX", "BK", "YHOO", "BMO", "REGN", "PSX", "SCHW", "AET", "RTN", "ORAN", "SYT", "QQQ", "TGT", "NFLX", "HPE", "BCS", "GIS", "NOC", "TRP", "PSA", "MCK", "HAL", "JD", "TMUS", "COF", "EBAY", "CME", "INFY", "NVDA", "YUM", "PRU", "ECL", "BAM", "CNQ", "ANTM", "CCL", "EMR", "CUK", "CI", "TRV", "PX", "NTES", "STZ", "ICE", "NOK", "ATVI", "BSX", "PCG", "EL", "CCI", "BX", "CM", "VMW", "TRI", "TSLA", "AFL", "ALXN", "DAL", "INTU", "JCI", "KR", "CS", "RBS", "WM", "TSN", "CSX", "MNST", "K", "ORLY", "S", "TTM", "HPQ", "DE", "BAX", "HUM", "EW", "SHW", "LNKD", "ALL", "EQIX", "ROST", "DISH", "EA", "EIX", "CAH", "APA", "ED", "GLW", "VFC", "TAP", "DFS", "ERIC", "LUV", "STJ", "MYL", "CBS", "CP", "STI", "MCO", "LB", "HSY", "HRL", "DG", "SIRI", "IP", "NMR", "CAG", "DLPH", "MU", "DLTR", "SWK", "AAL", "WDAY", "MTB", "PGR", "DB", "AMTD", "RYAAY", "IR", "CPB", "DPS", "LVLT", "ADSK", "TROW", "UA", "CLX", "UAL", "SJM", "EFX", "SYMC", "HES", "GPC", "ULTA", "CTL", "IBKR", "MJN", "MGM", "VIAB", "LH", "AWK", "PANW", "VRSK", "CTXS", "GWW", "PRGO", "WCN", "TWTR", "IVZ", "AGU", "CFG", "WHR", "CTAS", "MKC", "BBY", "RF", "DGX", "GPN", "CMG", "NDAQ", "FAST", "LLL", "IFF", "M", "MAT", "AAP", "TXT", "QSR", "WYNN", "XRX", "COH", "KMX", "HAS", "DISCA", "WU", "DISCK", "HBI", "WWAV", "MBLY", "HOG", "LPL", "FRO", "INGR", "ALLY", "GRMN", "AKAM", "ARMK", "RACE", "WFM", "TIF", "TSCO", "FBHS", "FL", "JWN", "TRIP", "LULU", "GPS", "JBHT", "PVH", "MELI", "LEA", "KORS", "RAD", "GT", "VRSN", "MPEL", "RJF", "NWS", "NWSA", "PSO", "ALK", "FCAU", "RL", "KSS", "ETFC", "PKG", "WYN", "DPZ", "YNDX", "TEAM", "DKS", "GIL", "BBBY", "QGEN", "URI", "VIP", "TRU", "PKI", "TWLO", "BURL", "HAR", "PF", "MTN", "VOYA", "SIG", "GDDY", "AMD", "WTR", "HLF", "WOOF", "JBLU", "SPLS", "SHLX", "PPC", "WCG", "USFD", "GRA", "CG", "POST", "BEAV", "MIK", "SIX", "SMG", "ATHN", "MAN", "PII", "AN", "IGT", "BUFF", "TOL", "CLB", "ALSN", "CASY", "PNRA", "CRI", "TPX", "WSM", "CNK", "GGG", "Z", "URBN", "OSK", "PAG", "HUN", "HTZ", "RAX", "SBH", "FIT", "RBA", "LOGI", "SHOP", "BRKR", "AMCX", "LM", "CAR", "BGS", "CAB", "TIVO", "R", "MOH", "SUN", "RYN", "RGC", "AEO", "DFT", "CBRL", "ELLI", "JACK", "FLO", "LGF", "SKX", "STRZA", "YELP", "TDS", "JCP", "SFM", "SLCA", "GME", "STAY", "TXRH", "ZEN", "ENR", "EAT", "BWLD", "WEN", "SWFT", "VA", "JNS", "AVP", "CAKE", "PBH", "CREE", "KATE", "GPRO", "LAD", "LOGM", "KNX", "LXK", "VIRT", "SHOO", "SAFM", "BIG", "SFUN", "BID", "VSH", "THC", "HSNI", "CALM", "DECK", "BLMN", "UNFI", "DDS", "PRTY", "ODP", "NTGR", "IMAX", "NYT", "WBMD", "FIG", "CATM", "KS", "BCO", "ZG", "DBD", "AAN", "MLHR", "PAY", "HMHC", "SEM", "DSW", "CZR", "PLAY", "STMP", "WWE", "CHS", "LOCK", "SWHC", "HTLD", "DF", "TIME", "BLOX", "VG", "GIII", "PLCE", "GNC", "MSGN", "SKYW", "RH", "GCI", "KBH", "LQ", "GPI", "TGI", "SONC", "LZB", "HW", "SHLD", "SHAK", "GES", "KFY", "AIRM", "SVU", "SCOR", "SCSS", "SEAS", "PDS", "ELY", "ANF", "TREE", "GOGO", "TQQQ", "SFS", "HRI", "HIBB", "EXPR", "WIN", "WING", "BKS", "DENN", "TRUE", "USCR", "FRGI", "RCII", "CROX", "SQQQ", "LL", "LOCO", "TACO", "ZUMZ", "BIS"
            };

            var tasks = symbols.Select(symbol => ProcessStockSymbol(symbol)).ToList();
            var results = await Task.WhenAll(tasks);
            var dayWinners = results.Where(stockResult => stockResult.Item2 == true).Select(stockResult => stockResult.Item1).ToList();
            var weekWinners = results.Where(stockResult => stockResult.Item3 == true).Select(stockResult => stockResult.Item1).ToList();
            var monthWinners = results.Where(stockResult => stockResult.Item4 == true).Select(stockResult => stockResult.Item1).ToList();
/*
            var dayPoss = results.Where(stockResult => stockResult.Item5 == true).Select(stockResult => stockResult.Item1).ToList();
            var weekPoss = results.Where(stockResult => stockResult.Item6 == true).Select(stockResult => stockResult.Item1).ToList();
            var monthPoss = results.Where(stockResult => stockResult.Item7 == true).Select(stockResult => stockResult.Item1).ToList();
*/
            Console.WriteLine($"Day Winners: {string.Join(" ", dayWinners)}");
            Console.WriteLine($"Week Winners: {string.Join(" ", weekWinners)}");
            Console.WriteLine($"Month Winners: {string.Join(" ", monthWinners)}");
/*
            Console.WriteLine($"Day Poss: {string.Join(" ", dayPoss)}");
            Console.WriteLine($"Week Poss: {string.Join(" ", weekPoss)}");
            Console.WriteLine($"Month Poss: {string.Join(" ", monthPoss)}");
            */
        }

        static async Task<Tuple<string, bool, bool, 
        //bool, bool, bool, 
        bool>> ProcessStockSymbol(string symbol)
        {
            String sDate = DateTime.Now.ToString();
            DateTime dateValue = (Convert.ToDateTime(sDate.ToString()));
            String dd = dateValue.Day.ToString();
            String mm = (dateValue.Month - 1).ToString();
            String yy = dateValue.Year.ToString();

            var url = $"http://chart.finance.yahoo.com/table.csv?s={symbol}&a=3&b=11&c=2016&d=" + mm + "&e=" + dd + "&f=" + yy + "&g=d&ignore=.csv";
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
                  //  ,
                /*    MatchesDayPoss(days),
                    MatchesWeekPoss(days),
                    MatchesMonthPoss(days)
                    */
                );
            }
        }

        static bool MatchesDayPattern(List<Day> days)
        {
            return (
               


/*
                days[0].close < days[1].close && days[0].close < days[0].open &&
                days[1].close < days[2].close && days[1].close < days[1].open &&
                days[2].close < days[3].close && days[2].close < days[2].open &&
                days[3].close < days[4].close && days[3].close < days[3].open &&
                days[4].close > days[5].close
*/

                days[0].close < days[1].close && days[0].close < days[0].open &&
                days[1].close < days[2].close && days[1].close < days[1].open &&
                days[2].close < days[3].close && days[2].close < days[2].open &&
                days[3].close > days[4].close 

                );
        }
               // int count = 40;  
                
        public static bool MatchesWeekPattern(List<Day> days)
        {

            /*
                int count = 40;  
                int dayOneWeeks;
                int dayTwoWeeks;
                int dayThreeWeeks;
                int dayFourWeeks;
                int dayFiveWeeks;
                int daySixWeeks;
                int found = 0;


                for (int i = 0; i <= count; i++)
                    {
                      //  string s = days[i].date.ToString();
                        DateTime dt         = DateTime.ParseExact(days[i].date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime dtPrevious = DateTime.ParseExact(days[i+1].date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        
                        if((dt - dtPrevious).TotalDays < 2 && found == 0)
                        {
                            dayOneWeeks = i;
                            found = found + 1;
                            
                        }
                        if((dt - dtPrevious).TotalDays < 2 && found == 1)
                        {
                            dayTwoWeeks = i;
                            found = found + 1;
                        }
                        if((dt - dtPrevious).TotalDays < 2 && found == 2)
                        {
                            dayThreeWeeks = i;
                            found = found + 1;
                        }
                        if((dt - dtPrevious).TotalDays < 2 && found == 3)
                        {
                            dayFourWeeks = i;
                            found = found + 1;
                        }
                        if((dt - dtPrevious).TotalDays < 2 && found == 4)
                        {
                            dayFiveWeeks = i;
                            found = found + 1;
                        }
                        if((dt - dtPrevious).TotalDays < 2 && found == 5)
                        {
                            daySixWeeks = i;
                            found = found + 1;
                        }
                        
                    }
                */
                return (
                days[0].close < days[5].close && days[0].close < days[4].open 
           /*     &&
                days[5].close < days[10].close && days[5].close < days[9].open &&
                days[10].close < days[15].close && days[10].close < days[14].open &&
                days[15].close < days[20].close && days[15].close < days[19].open &&
                days[20].close > days[25].close
           */
                );

                
         /*       return dayTwoWeeks;
                return dayThreeWeeks;
                return dayFourWeeks;
                return dayFiveWeeks;
                return daySixWeeks;
*/

        }

        static bool MatchesMonthPattern(List<Day> days)
        {
            int count = 120;  
                
                for (int i = 1; i <= count; i++)
                    {
                         //if datetime.days[i] < datetime.days[i + 1]
                         //set [i] as 1st of Month and [i+1] as End of Month
                         //stop when 6 numbers chosen
                    }



            return (
                days[10].close < days[31].close && days[10].close < days[30].open 
                /*
                &&
                days[31].close < days[54].close && days[31].close < days[53].open &&
                days[54].close < days[74].close && days[54].close < days[73].open &&
                days[74].close < days[96].close && days[74].close < days[95].open 
             && days[96].close > days[117].close
             */


                );
        }
/*        static bool MatchesDayPoss(List<Day> days)
        {
            return (
               
                days[0].close < days[1].close && days[0].close < days[0].open &&
                days[1].close < days[2].close && days[1].close < days[1].open &&
                days[2].close < days[3].close && days[2].close < days[2].open &&
                days[3].close > days[4].close 
                
                );
        }

        static bool MatchesWeekPoss(List<Day> days)
        {
                return (
                days[0].close < days[5].close && days[0].close < days[4].open &&
                days[5].close < days[10].close && days[5].close < days[9].open &&
                days[10].close < days[15].close && days[10].close < days[14].open &&
                days[15].close > days[20].close );

            
        }

        static bool MatchesMonthPoss(List<Day> days)
        {
            return (
                days[10].close < days[31].close && days[10].close > days[30].open 
                
                &&
                days[31].close < days[54].close && days[31].close < days[53].open &&
                days[54].close < days[74].close && days[54].close < days[73].open &&
                days[74].close > days[96].close 



             
             


                );
        }

*/


    }
}
