using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;
        private readonly CurrencyService _currencyService;
        private readonly CryptoService _cryptoService;
        private readonly IDialogService _dialogService;

        [ObservableProperty] private string btcPriceInfo = "Ładowanie...";
        [ObservableProperty] private Expense? selectedExpense;
        [ObservableProperty] private decimal currentMonthTotal;
        [ObservableProperty] private decimal lastMonthTotal;
        [ObservableProperty] private string comparisonText = "Brak danych";
        [ObservableProperty] private string comparisonColor = "#888888";

        public ObservableCollection<Category> CategoryStats { get; } = new();
        public ObservableCollection<Expense> Expenses { get; } = new();

        public MainViewModel(DatabaseService db, CurrencyService curr, CryptoService crypto, IDialogService dialog)
        {
            _dbService = db;
            _currencyService = curr;
            _cryptoService = crypto;
            _dialogService = dialog;

            _ = LoadInitialData();
        }

        private async Task LoadInitialData()
        {
            await LoadData();
            await LoadBtcPrice();
        }

        [RelayCommand]
        private void OpenCategoryManager()
        {
            var vm = new CategoryListViewModel(_dbService);

            _dialogService.ShowCategoryList(vm);

            CalculateStatistics();
        }

        [RelayCommand]
        private async Task LoadData()
        {
            var list = await _dbService.GetAllAsync();
            Expenses.Clear();
            foreach (var item in list.OrderByDescending(x => x.Date))
                Expenses.Add(item);

            CalculateStatistics();
        }

        [RelayCommand]
        private async Task AddExpense()
        {
            var formVm = new ExpenseFormViewModel(_currencyService, _dbService);
            if (_dialogService.ShowExpenseDialog(formVm))
            {
                try
                {
                    var newExpense = formVm.GetExpense();
                    await _dbService.AddAsync(newExpense);
                    Expenses.Insert(0, newExpense);
                    CalculateStatistics();
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage(ex.Message, "Błąd");
                }
            }
        }

        [RelayCommand]
        private async Task DeleteExpense()
        {
            if (SelectedExpense == null) return;
            await _dbService.DeleteAsync(SelectedExpense);
            Expenses.Remove(SelectedExpense);
            CalculateStatistics();
        }

        [RelayCommand]
        private async Task AddCategory()
        {
            var vm = new AddCategoryViewModel();
            if (_dialogService.ShowCategoryDialog(vm))
            {
                var newCat = new Category { Name = vm.CategoryName };
                await _dbService.AddCategoryAsync(newCat);
                CalculateStatistics(); 
                _dialogService.ShowMessage($"Dodano nową kategorię: {vm.CategoryName}", "Sukces");
            }
        }

        [RelayCommand]
        private async Task DeleteCategory(Category category)
        {
            if (category == null || category.Name == "Inne") return;

            var result = MessageBox.Show($"Czy usunąć kategorię '{category.Name}'?", "Potwierdzenie", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _dbService.DeleteCategoryByNameAsync(category.Name);
                CalculateStatistics();
            }
        }


        private async Task LoadBtcPrice()
        {
            try
            {
                var btc = await _cryptoService.GetBtcPriceInEurAsync();
                BtcPriceInfo = btc > 0 ? $"{btc:N0} €" : "Błąd API";

            }
            catch { BtcPriceInfo = "Błąd połączenia"; }
        }

        private void CalculateStatistics()
        {
            var now = DateTime.Now;

            CurrentMonthTotal = Expenses
                .Where(e => e.Date.Month == now.Month && e.Date.Year == now.Year)
                .Sum(e => e.AmountInPln);

            var lastMonthDate = now.AddMonths(-1);
            LastMonthTotal = Expenses
                .Where(e => e.Date.Month == lastMonthDate.Month && e.Date.Year == lastMonthDate.Year)
                .Sum(e => e.AmountInPln);

            UpdateComparison();
            UpdateCategorySummary();
        }

        private void UpdateComparison()
        {
            if (LastMonthTotal <= 0)
            {
                ComparisonText = "Brak danych z zeszłego miesiąca";
                ComparisonColor = "#888888";
                return;
            }

            decimal diff = CurrentMonthTotal - LastMonthTotal;
            decimal percent = (diff / LastMonthTotal) * 100;

            if (diff > 0)
            {
                ComparisonText = $"⬆ o {percent:N1}% więcej niż miesiąc temu";
                ComparisonColor = "#D9534F";
            }
            else
            {
                ComparisonText = $"⬇ o {Math.Abs(percent):N1}% mniej niż miesiąc temu";
                ComparisonColor = "#5CB85C";
            }
        }

        private void UpdateCategorySummary()
        {
            var stats = Expenses
                .GroupBy(e => e.Category)
                .Select(g => new Category
                {
                    Name = g.Key,
                    Total = g.Sum(e => e.AmountInPln)
                })
                .OrderByDescending(x => x.Total)
                .ToList();

            CategoryStats.Clear();
            foreach (var s in stats) CategoryStats.Add(s);
        }
    }
}