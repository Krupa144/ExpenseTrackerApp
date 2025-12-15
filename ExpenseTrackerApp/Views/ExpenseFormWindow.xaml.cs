using ExpenseTrackerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpenseTrackerApp.Views
{
    public partial class ExpenseFormWindow : Window
    {
        private readonly ExpenseFormViewModel? _viewModel;

        public ExpenseFormWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Window_MouseDown;
        }

        public ExpenseFormWindow(ExpenseFormViewModel viewModel) : this()
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null) return;

            _viewModel.Validate();

            if (_viewModel.HasErrors)
            {
                MessageBox.Show("Proszę poprawić błędy w formularzu przed zapisem.",
                                "Błąd walidacji",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}