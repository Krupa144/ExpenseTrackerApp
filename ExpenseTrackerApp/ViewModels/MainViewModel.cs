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
        [ObservableProperty] private string comparisonColor = "Gray"; 

        public ObservableCollection<CategorySummary> CategoryStats { get; } = new();

        public ObservableCollection<Expense> Expenses { get; } = new();

        public MainViewModel(DatabaseService db, CurrencyService curr, CryptoService crypto, IDialogService dialog)
        {
            _dbService = db;
            _currencyService = curr;
            _cryptoService = crypto;
            _dialogService = dialog;

            Task.Run(LoadApiData);
            LoadDataCommand.Execute(null);
        }

        private async Task LoadApiData()
        {
            var btc = await _cryptoService.GetBtcPriceInPlnAsync();
            BtcPriceInfo = btc > 0 ? $"{btc:N0} PLN" : "Błąd";
        }

        private void CalculateStatistics()
        {
            var now = DateTime.Now;
            var currentMonth = now.Month;
            var currentYear = now.Year;

            var prevMonthDate = now.AddMonths(-1);
            var prevMonth = prevMonthDate.Month;
            var prevYear = prevMonthDate.Year;

            CurrentMonthTotal = Expenses
                .Where(e => e.Date.Month == currentMonth && e.Date.Year == currentYear)
                .Sum(e => e.AmountInPln);

            LastMonthTotal = Expenses
                .Where(e => e.Date.Month == prevMonth && e.Date.Year == prevYear)
                .Sum(e => e.AmountInPln);

            decimal diff = CurrentMonthTotal - LastMonthTotal;
            if (LastMonthTotal == 0)
            {
                ComparisonText = "Brak danych z poprzedniego miesiąca";
                ComparisonColor = "Gray";
            }
            else
            {
                decimal percent = (diff / LastMonthTotal) * 100;
                if (diff > 0)
                {
                    ComparisonText = $"⬆ Wydajesz o {Math.Abs(percent):N1}% więcej niż miesiąc temu";
                    ComparisonColor = "#D9534F"; 
                }
                else
                {
                    ComparisonText = $"⬇ Oszczędziłeś {Math.Abs(percent):N1}% względem poprzedniego miesiąca";
                    ComparisonColor = "#5CB85C"; 
                }
            }

            var stats = Expenses
                .GroupBy(e => e.Category)
                .Select(g => new CategorySummary { Name = g.Key, Total = g.Sum(e => e.AmountInPln) })
                .OrderByDescending(x => x.Total)
                .ToList();

            CategoryStats.Clear();
            foreach (var s in stats) CategoryStats.Add(s);
        }

        [RelayCommand]
        private async Task LoadData()
        {
            Expenses.Clear();
            var list = await _dbService.GetAllAsync();
            foreach (var item in list.OrderByDescending(x => x.Date)) Expenses.Add(item);
            CalculateStatistics();
        }

        [RelayCommand]
        private async Task AddExpense()
        {
            var formVm = new ExpenseFormViewModel(_currencyService);
            if (_dialogService.ShowExpenseDialog(formVm))
            {
                var newExpense = formVm.GetExpense();
                await _dbService.AddAsync(newExpense);
                Expenses.Insert(0, newExpense); 
                CalculateStatistics();
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
    }
    public class CategorySummary
    {
        public string Name { get; set; } = "";
        public decimal Total { get; set; }
    }
}