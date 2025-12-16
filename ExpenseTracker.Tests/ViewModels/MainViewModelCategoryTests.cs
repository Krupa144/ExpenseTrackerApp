using Xunit;
using System;
using System.Linq;
using ExpenseTrackerApp.ViewModels;
using ExpenseTrackerApp.Models;

namespace ExpenseTracker.Tests.ViewModels
{
    public class MainViewModelCategoryTests
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
        public void Groups_Expenses_By_Category()
        {
            var vm = CreateViewModel();

            vm.Expenses.Add(new Expense { Category = "Jedzenie", AmountInPln = 100, Date = DateTime.Now });
            vm.Expenses.Add(new Expense { Category = "Jedzenie", AmountInPln = 50, Date = DateTime.Now });
            vm.Expenses.Add(new Expense { Category = "Transport", AmountInPln = 30, Date = DateTime.Now });

            Calculate(vm);

            var food = vm.CategoryStats.First(c => c.Name == "Jedzenie");
            var transport = vm.CategoryStats.First(c => c.Name == "Transport");

            Assert.Equal(150, food.Total);
            Assert.Equal(30, transport.Total);
        }
    }
}
