using Xunit;
using System;
using ExpenseTrackerApp.ViewModels;
using ExpenseTrackerApp.Models;

namespace ExpenseTracker.Tests.ViewModels
{
    public class MainViewModelComparisonTests
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
        public void Shows_Increase_When_CurrentMonth_Is_Higher()
        {
            var vm = CreateViewModel();

            vm.Expenses.Add(new Expense { Date = DateTime.Now, AmountInPln = 200 });
            vm.Expenses.Add(new Expense { Date = DateTime.Now.AddMonths(-1), AmountInPln = 100 });

            Calculate(vm);

            Assert.Contains("więcej", vm.ComparisonText);
            Assert.Equal("#D9534F", vm.ComparisonColor);
        }

        [Fact]
        public void Shows_NoData_When_LastMonth_Is_Zero()
        {
            var vm = CreateViewModel();

            vm.Expenses.Add(new Expense { Date = DateTime.Now, AmountInPln = 100 });

            Calculate(vm);

            Assert.Equal("Brak danych z zeszłego miesiąca", vm.ComparisonText);
            Assert.Equal("#888888", vm.ComparisonColor);
        }
    }
}
