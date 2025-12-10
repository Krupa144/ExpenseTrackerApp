using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExpenseTrackerApp.Services
{
    public class CurrencyService
    {
        private decimal _currentRate = 4.3m;

        private readonly System.Timers.Timer _timer;
        private readonly string _apiUrl;

        public decimal CurrentRate => _currentRate;
        public event Action<decimal>? RateUpdated;

        public CurrencyService()
        {
            _apiUrl = Environment.GetEnvironmentVariable("CURRENCY_API_URL")
                      ?? "http://api.nbp.pl/api/exchangerates/rates/a/eur/?format=json";

            _timer = new System.Timers.Timer();
            _timer.Elapsed += async (s, e) => await FetchRateAsync();

            SetupTimerForNineAM();

            Task.Run(FetchRateAsync);
        }

        private void SetupTimerForNineAM()
        {
            DateTime now = DateTime.Now;
            DateTime nineAm = now.Date.AddHours(9);
            if (now > nineAm) nineAm = nineAm.AddDays(1);

            double msUntilNine = (nineAm - now).TotalMilliseconds;

            if (msUntilNine <= 0) msUntilNine = 1000;

            _timer.Interval = msUntilNine;
            _timer.AutoReset = false;
            _timer.Start();
        }

        private async Task FetchRateAsync()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(_apiUrl);
                var data = JObject.Parse(json);

                var rate = data["rates"]?[0]?["mid"]?.Value<decimal>();

                if (rate.HasValue)
                {
                    _currentRate = rate.Value;
                    RateUpdated?.Invoke(_currentRate);
                }

                if (_timer.AutoReset == false)
                {
                    _timer.Interval = 24 * 60 * 60 * 1000;
                    _timer.AutoReset = true;
                    _timer.Start();
                }
            }
            catch
            {

            }
        }
    }
}