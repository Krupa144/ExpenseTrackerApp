using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace ExpenseTrackerApp.ViewModels
{
    public partial class CategoryListViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        public ObservableCollection<Category> Categories { get; } = new();

        public CategoryListViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            _ = LoadCategories();
        }

        private async Task LoadCategories()
        {
            var list = await _dbService.GetCategoriesAsync();
            Categories.Clear();
            foreach (var cat in list) Categories.Add(cat);
        }

        [RelayCommand]
        private async Task DeleteCategory(Category category)
        {
            if (category == null || category.Name == "Inne") return;

            if (MessageBox.Show($"Czy na pewno usunąć kategorię '{category.Name}'?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _dbService.DeleteCategoryByNameAsync(category.Name);
                Categories.Remove(category);
            }
        }
    }
}