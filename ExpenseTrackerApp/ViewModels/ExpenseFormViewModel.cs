using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class ExpenseFormViewModel : ObservableValidator
    {
        private readonly CurrencyService _currencyService;

        [ObservableProperty]
        [Required(ErrorMessage = "Wpisz co kupiłeś")]
        private string title = string.Empty;

        [ObservableProperty]
        [Range(0.01, 1000000, ErrorMessage = "Kwota > 0")]
        private string amountString = "";

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        [ObservableProperty]
        private string selectedCurrency = "PLN";

        [ObservableProperty]
        private string selectedCategory = "Inne";

        public ObservableCollection<string> AvailableCurrencies { get; } = new();

        public ObservableCollection<string> Categories { get; } = new()
        {
            "Dom i Rachunki",
            "Jedzenie",
            "Sport i Zdrowie",
            "Transport",
            "Subskrypcje",
            "Rozrywka",
            "Inne"
        };

        public ExpenseFormViewModel(CurrencyService currencyService)
        {
            _currencyService = currencyService;
            foreach (var c in _currencyService.GetAvailableCurrencies()) AvailableCurrencies.Add(c);
        }

        public Expense GetExpense()
        {
            ValidateAllProperties();
            if (HasErrors) throw new ValidationException("Błędy w formularzu");
            if (!decimal.TryParse(AmountString, out decimal originalAmount)) throw new ValidationException("Zła kwota");

            decimal rate = _currencyService.GetRate(SelectedCurrency);

            return new Expense
            {
                Title = Title,
                Date = Date,
                OriginalAmount = originalAmount,
                Currency = SelectedCurrency,
                ExchangeRateUsed = rate,
                RateDate = DateTime.Now, 
                Category = SelectedCategory, 
                AmountInPln = originalAmount * rate
            };
        }
    }
}