using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;
        private readonly CurrencyService _currencyService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private decimal currentEurRate;

        [ObservableProperty]
        private Expense? selectedExpense;

        public ObservableCollection<Expense> Expenses { get; } = new();

        public MainViewModel(DatabaseService db, CurrencyService curr, IDialogService dialog)
        {
            _dbService = db;
            _currencyService = curr;
            _dialogService = dialog;

            CurrentEurRate = _currencyService.CurrentRate;
            _currencyService.RateUpdated += rate => CurrentEurRate = rate;

            LoadDataCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadData()
        {
            Expenses.Clear();
            var list = await _dbService.GetAllAsync();
            foreach (var item in list)
            {
                item.AmountInEur = item.Amount / (CurrentEurRate > 0 ? CurrentEurRate : 1);
                Expenses.Add(item);
            }
        }

        [RelayCommand]
        private async Task AddExpense()
        {
            var formVm = new ExpenseFormViewModel();
            if (_dialogService.ShowExpenseDialog(formVm))
            {
                try
                {
                    var newExpense = formVm.GetExpense();
                    newExpense.AmountInEur = newExpense.Amount / (CurrentEurRate > 0 ? CurrentEurRate : 1);

                    await _dbService.AddAsync(newExpense);
                    Expenses.Add(newExpense);
                }
                catch (System.Exception ex)
                {
                    _dialogService.ShowMessage($"Błąd: {ex.Message}", "Błąd");
                }
            }
        }

        [RelayCommand]
        private async Task DeleteExpense()
        {
            if (SelectedExpense == null) return;
            await _dbService.DeleteAsync(SelectedExpense);
            Expenses.Remove(SelectedExpense);
        }
    }
}