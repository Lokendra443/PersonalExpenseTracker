using PersonalExpenseTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public class DashboardService : IDashboardService
    {
        string dbPath = @"C:\Users\loken\Desktop\db.db3";
        private SQLiteConnection _conn;

        public DashboardService()
        {
            _conn = new SQLiteConnection(dbPath);
            _conn.CreateTable<DashboardStats>(); // Create the table if it doesn't exist
        }

        public void UpdateDashboardStats()
        {
            // Aggregate stats from transactions
            var totalInflows = _conn.Table<Transaction>().Where(t => t.Type == "inflow").Sum(t => t.Amount);
            var totalOutflows = _conn.Table<Transaction>().Where(t => t.Type == "outflow").Sum(t => t.Amount);
            var totalDebts = _conn.Table<Transaction>().Where(t => t.Type == "debt").Sum(t => t.Amount);
            var clearedDebts = _conn.Table<Transaction>().Where(t => t.Type == "debt" && t.Status == "Cleared").Sum(t => t.Amount);
            var remainingDebts = totalDebts - clearedDebts;
            var netBalance = totalInflows + totalDebts - totalOutflows;

            // Create a new DashboardStats record
            var dashboardStats = new DashboardStats
            {
                TotalInflows = totalInflows,
                TotalOutflows = totalOutflows,
                TotalDebts = totalDebts,
                ClearedDebts = clearedDebts,
                RemainingDebts = remainingDebts,
                NetBalance = netBalance,
                Date = DateTime.Now.Date // Store the current date
            };

            // Insert or update the stats for today
            var existingStats = _conn.Table<DashboardStats>().FirstOrDefault(ds => ds.Date == DateTime.Now.Date);
            if (existingStats != null)
            {
                // Update stats for today
                existingStats.TotalInflows = totalInflows;
                existingStats.TotalOutflows = totalOutflows;
                existingStats.TotalDebts = totalDebts;
                existingStats.ClearedDebts = clearedDebts;
                existingStats.RemainingDebts = remainingDebts;
                existingStats.NetBalance = netBalance;
                _conn.Update(existingStats);
            }
            else
            {
                // Insert new stats for today
                _conn.Insert(dashboardStats);
            }
        }


        public List<Transaction> GetPendingDebts()
        {
            return _conn.Table<Transaction>()
                        .Where(t => t.Type == "debt" && t.Status == "Pending")
                        .OrderBy(t => t.DueDate)
                        .ToList();
        }
    }
}
