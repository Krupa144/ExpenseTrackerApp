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

        public string Category { get; set; } = string.Empty;

        public decimal OriginalAmount { get; set; }

        public string Currency { get; set; } = "PLN";

        public decimal AmountInPln { get; set; }

        public decimal ExchangeRateUsed { get; set; }

        public DateTime RateDate { get; set; }

        public DateTime Date { get; set; }
    }
}