using Xunit;
using System;
using ExpenseTrackerApp.ViewModels;
using ExpenseTrackerApp.Models;

namespace ExpenseTracker.Tests.ViewModels
{
    public class MainViewModelStatisticsTests
    {
        private MainViewModel CreateViewModel()
            => new MainViewModel(null!, null!, null!, null!);

        private void Calculate(MainViewModel vm)
            => typeof(MainViewModel)
                .GetMethod("CalculateStatistics",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance)!
                .Invoke(vm, null);

        [Fact]
        public void Calculates_CurrentMonthTotal_Correctly()
        {
            var vm = CreateViewModel();

            vm.Expenses.Add(new Expense { Date = DateTime.Now, AmountInPln = 100 });
            vm.Expenses.Add(new Expense { Date = DateTime.Now, AmountInPln = 200 });

            Calculate(vm);

            Assert.Equal(300, vm.CurrentMonthTotal);
        }
    }
}
