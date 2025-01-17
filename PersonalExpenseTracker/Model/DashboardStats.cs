using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Model
{
    public class DashboardStats
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal TotalInflows { get; set; }
        public decimal TotalOutflows { get; set; }
        public decimal TotalDebts { get; set; }
        public decimal ClearedDebts { get; set; }
        public decimal RemainingDebts { get; set; }
        public decimal NetBalance { get; set; }

        // Store the date for which stats are calculated
        public DateTime Date { get; set; }
    }
}
