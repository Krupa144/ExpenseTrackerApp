using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpenseTrackerApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbName = Environment.GetEnvironmentVariable("DB_FILENAME") ?? "expenses.db";

            optionsBuilder.UseSqlite($"Data Source={dbName}");
        }
    }
}