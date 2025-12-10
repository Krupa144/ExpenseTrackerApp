using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerApp.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool ShowExpenseDialog(ViewModels.ExpenseFormViewModel viewModel);
    }

    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title)
        {
            System.Windows.MessageBox.Show(message, title);
        }

        public bool ShowExpenseDialog(ViewModels.ExpenseFormViewModel viewModel)
        {
            var win = new Views.ExpenseFormWindow();
            win.DataContext = viewModel;
            return win.ShowDialog() == true;
        }
    }
}
