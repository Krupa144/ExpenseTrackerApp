using System.Windows;
using ExpenseTrackerApp.ViewModels;
using ExpenseTrackerApp.Views;

namespace ExpenseTrackerApp.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool ShowExpenseDialog(ExpenseFormViewModel viewModel);
        bool ShowCategoryDialog(AddCategoryViewModel viewModel);
        void ShowCategoryList(CategoryListViewModel viewModel);
    }

    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title);
        }

        public bool ShowExpenseDialog(ExpenseFormViewModel viewModel)
        {
            var win = new ExpenseFormWindow(viewModel);
            if (Application.Current?.MainWindow != null)
                win.Owner = Application.Current.MainWindow;

            return win.ShowDialog() == true;
        }

        public bool ShowCategoryDialog(AddCategoryViewModel viewModel)
        {
            var win = new AddCategoryWindow(viewModel);

            if (Application.Current?.MainWindow != null)
                win.Owner = Application.Current.MainWindow;

            return win.ShowDialog() ?? false;
        }

        public void ShowCategoryList(CategoryListViewModel viewModel)
        {
            var win = new CategoryListWindow(viewModel);

            if (Application.Current?.MainWindow != null)
                win.Owner = Application.Current.MainWindow;

            win.ShowDialog();
        }
    }
}