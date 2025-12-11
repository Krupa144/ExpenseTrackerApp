using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApp.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Category { get; set; } = "🛍️ Inne";

        public decimal OriginalAmount { get; set; }

        public string Currency { get; set; } = "PLN";

        public decimal ExchangeRateUsed { get; set; } = 1.0m;

        public DateTime RateDate { get; set; } = DateTime.Now;

        public decimal AmountInPln { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}