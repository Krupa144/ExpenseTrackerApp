using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExpenseTrackerApp.Services
{
    public class CurrencyService
    {
        private const string ApiUrl = "http://api.nbp.pl/api/exchangerates/tables/a/?format=json";

        private Dictionary<string, decimal> _rates = new();

        public event Action? RatesUpdated;

        public CurrencyService()
        {
            Task.Run(FetchRatesAsync);
        }

        public decimal GetRate(string currencyCode)
        {
            if (currencyCode == "PLN") return 1.0m;
            return _rates.ContainsKey(currencyCode) ? _rates[currencyCode] : 1.0m;
        }

        public IEnumerable<string> GetAvailableCurrencies()
        {
            var list = new List<string> { "PLN" }; 
            list.AddRange(_rates.Keys);
            return list;
        }

        private async Task FetchRatesAsync()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(ApiUrl);
                var array = JArray.Parse(json);
                var ratesArray = array[0]["rates"] as JArray;

                if (ratesArray != null)
                {
                    foreach (var item in ratesArray)
                    {
                        var code = item["code"]?.ToString();
                        var mid = item["mid"]?.Value<decimal>();

                        if (!string.IsNullOrEmpty(code) && mid.HasValue)
                        {
                            _rates[code] = mid.Value;
                        }
                    }
                    if (!_rates.ContainsKey("USD")) _rates["USD"] = 4.0m;
                    if (!_rates.ContainsKey("EUR")) _rates["EUR"] = 4.3m;

                    RatesUpdated?.Invoke();
                }
            }
            catch
            {
                _rates["EUR"] = 4.3m;
                _rates["USD"] = 3.9m;
            }
        }
    }
}