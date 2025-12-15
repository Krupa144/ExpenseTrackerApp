using System.Windows;
using System.Windows.Input;
using ExpenseTrackerApp.ViewModels; 

namespace ExpenseTrackerApp.Views
{
    public partial class AddCategoryWindow : Window
    {
        private readonly AddCategoryViewModel _viewModel;

        public AddCategoryWindow(AddCategoryViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed) DragMove();
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Validate();
            if (_viewModel.HasErrors)
            {
                return;
            }

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}