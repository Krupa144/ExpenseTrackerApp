using System;
using System.ComponentModel.DataAnnotations; 
using CommunityToolkit.Mvvm.ComponentModel; 

namespace ExpenseTrackerApp.ViewModels
{
    public partial class AddCategoryViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo] 
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [MinLength(3, ErrorMessage = "Minimum 3 znaki")]
        private string categoryName = string.Empty;

        public void Validate() => ValidateAllProperties();
    }
}