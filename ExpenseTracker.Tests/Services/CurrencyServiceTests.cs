using Xunit;
using ExpenseTrackerApp.Services;
using System.Linq;

namespace ExpenseTracker.Tests.Services
{
    public class CurrencyServiceTests
    {
        [Fact]
        public void GetRate_ForPLN_ReturnsOne()
        {
            var service = new CurrencyService();

            var rate = service.GetRate("PLN");

            Assert.Equal(1.0m, rate);
        }

        [Fact]
        public void GetRate_ForUnknownCurrency_ReturnsDefaultOne()
        {
            var service = new CurrencyService();

            var rate = service.GetRate("XXX");

            Assert.Equal(1.0m, rate);
        }

        [Fact]
        public void AvailableCurrencies_AlwaysContainsPLN()
        {
            var service = new CurrencyService();

            var currencies = service.GetAvailableCurrencies();

            Assert.Contains("PLN", currencies);
        }
    }
}
