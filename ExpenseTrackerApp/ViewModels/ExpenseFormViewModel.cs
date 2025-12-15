using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class ExpenseFormViewModel : ObservableValidator
    {
        private readonly CurrencyService _currencyService;
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Pole 'Nazwa' jest wymagane")]
        [MinLength(3, ErrorMessage = "Minimum 3 znaki")]
        private string title = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Podaj kwotę")]
        [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Nieprawidłowy format kwoty (np. 10.50)")]
        private string amountString = "0";

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        [ObservableProperty]
        private string selectedCurrency = "PLN";

        [ObservableProperty]
        private string selectedCategory = string.Empty;

        public ObservableCollection<string> AvailableCurrencies { get; } = new();
        public ObservableCollection<string> Categories { get; } = new();

        public ExpenseFormViewModel(CurrencyService currencyService, DatabaseService dbService)
        {
            _currencyService = currencyService;
            _dbService = dbService;

            foreach (var c in _currencyService.GetAvailableCurrencies())
                AvailableCurrencies.Add(c);

            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var dbCategories = await _dbService.GetCategoriesAsync();

            Categories.Clear();
            foreach (var cat in dbCategories)
            {
                Categories.Add(cat.Name);
            }

            if (!Categories.Any())
            {
                Categories.Add("Inne");
            }

            SelectedCategory = Categories.Contains("Inne") ? "Inne" : Categories.First();
        }

        public void Validate() => ValidateAllProperties();

        public Expense GetExpense()
        {
            ValidateAllProperties();
            if (HasErrors)
                throw new ValidationException("Formularz zawiera błędy.");

            string normalizedAmount = AmountString.Replace(',', '.');
            if (!decimal.TryParse(normalizedAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal originalAmount))
                throw new ValidationException("Wprowadzona kwota nie jest liczbą.");

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