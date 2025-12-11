using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExpenseTrackerApp.Services
{
    public class CryptoService
    {
        private const string ApiUrl = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=pln";

        public async Task<decimal> GetBtcPriceInPlnAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "ExpenseTrackerApp");

                var json = await client.GetStringAsync(ApiUrl);
                var data = JObject.Parse(json);

                var price = data["bitcoin"]?["pln"]?.Value<decimal>();

                return price ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}