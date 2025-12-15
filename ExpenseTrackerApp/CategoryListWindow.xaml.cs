using System.Windows;
using ExpenseTrackerApp.ViewModels;

namespace ExpenseTrackerApp.Views
{
    /// <summary>
    /// Logika interakcji dla klasy CategoryListWindow.xaml
    /// </summary>
    public partial class CategoryListWindow : Window
    {
        private readonly CategoryListViewModel _viewModel;

        public CategoryListWindow(CategoryListViewModel viewModel)
        {
            InitializeComponent();
            
            // Przypisujemy ViewModel do pola i jako DataContext okna
            _viewModel = viewModel;
            this.DataContext = _viewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            // Zamyka okno menedżera kategorii
            this.Close();
        }
    }
}