using CommunityToolkit.Mvvm.ComponentModel;
using ExpenseTrackerApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class ExpenseFormViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required(ErrorMessage = "Tytuł wymagany")]
        [MinLength(3, ErrorMessage = "Minimum 3 znaki")]
        private string title = string.Empty;

        [ObservableProperty]
        [Range(0.01, 1000000, ErrorMessage = "Kwota musi być dodatnia")]
        private string amountString = "0";

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        public Expense GetExpense()
        {
            ValidateAllProperties();
            if (HasErrors) throw new ValidationException("Formularz zawiera błędy.");

            if (!decimal.TryParse(AmountString, out decimal amount))
                throw new ValidationException("Nieprawidłowa kwota.");

            return new Expense { Title = Title, Amount = amount, Date = Date };
        }
    }
}