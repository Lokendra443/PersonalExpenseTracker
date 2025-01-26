using PersonalExpenseTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public class TransactionService : ITransactionService
    {

        string dbPath = @"C:\Users\loken\Desktop\db.db3";
        private SQLiteConnection _conn;

        public TransactionService()
        {
            _conn = new SQLiteConnection(dbPath);  // Ensure connection is established
            _conn.CreateTable<Transaction>();  // Create the transaction table if it doesn't exist
            _conn.CreateTable<TransactionTag>();  // Create the many-to-many relationship table if it doesn't exist
        }



        public void AddTransaction(Transaction transaction, List<int> tagIds)
        {
            


            // Validate for "debt" type transactions
            if (transaction.Type == "debt")
            {
                if (string.IsNullOrEmpty(transaction.SourceOfDebt) || transaction.DueDate == null)
                {
                    throw new ArgumentException("Source and Due Date are required for debts.");
                }
                transaction.Status = "Pending"; // Set default status for debts
            }

            // Insert the new transaction
            _conn.Insert(transaction);

            // Add associated tags
            foreach (var tagId in tagIds)
            {
                var transactionTag = new TransactionTag
                {
                    TransactionId = transaction.Id,
                    TagId = tagId
                };
                _conn.Insert(transactionTag);
            }
        }



        


        public List<Transaction> GetAllTransactions()
        {
            var transactions = _conn.Table<Transaction>().ToList();

            foreach (var transaction in transactions)
            {
                // Get all tag IDs associated with this transaction
                var tagIds = _conn.Table<TransactionTag>()
                                  .Where(tt => tt.TransactionId == transaction.Id)
                                  .Select(tt => tt.TagId)
                                  .ToList();

                // Get the actual tags and assign them to the transaction
                transaction.Tags = _conn.Table<Tag>()
                                        .Where(tag => tagIds.Contains(tag.Id))
                                        .ToList();
            }

            return transactions;
        }


        // Get transactions filtered data
        public List<Transaction> GetFilteredTransactions(
            string searchText,
            string transactionType,
            int? tagId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var transactions = GetAllTransactions();

            // Apply filters
            return transactions.Where(t =>
                (string.IsNullOrEmpty(searchText) || t.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(transactionType) || t.Type == transactionType) &&
                (!tagId.HasValue || t.Tags.Any(tag => tag.Id == tagId.Value)) &&
                (!startDate.HasValue || t.Date >= startDate.Value) &&
                (!endDate.HasValue || t.Date <= endDate.Value)
            ).ToList();
        }



        // Get all pending debts
        public List<Transaction> GetPendingDebts()
        {

            var transactions = GetAllTransactions();
            return transactions.Where(t => t.Type == "debt" && t.Status == "Pending").ToList();

        }



        // Update debt status (e.g., to 'Cleared')
        public bool UpdateDebtStatus(int transactionId, string newStatus)
        {
            var transaction = _conn.Table<Transaction>().FirstOrDefault(t => t.Id == transactionId);

            if (transaction != null)
            {
                transaction.Status = newStatus; // Update the status
                _conn.Update(transaction); // Save changes to the database
                return true; // Successfully updated
            }

            return false; // Transaction not found
        }


        public bool CheckSufficientBalanceForOutflow(decimal outflowAmount)
        {
            // Get the total inflows, debts, and outflows from the database
            var totalInflows = _conn.Table<Transaction>().Where(t => t.Type == "inflow").Sum(t => t.Amount);
            var totalDebts = _conn.Table<Transaction>().Where(t => t.Type == "debt").Sum(t => t.Amount);
            var totalOutflows = _conn.Table<Transaction>().Where(t => t.Type == "outflow").Sum(t => t.Amount);

            // Calculate the net balance (inflows + debts - outflows)
            var netBalance = totalInflows + totalDebts - totalOutflows;

            // Check if the current net balance is sufficient for the requested outflow amount
            return netBalance >= outflowAmount;
        }


        // Update an existing transaction
        public void UpdateTransaction(Transaction transaction)
        {
            // Ensure the transaction exists in the database before updating
            var existingTransaction = _conn.Table<Transaction>().FirstOrDefault(t => t.Id == transaction.Id);

            if (existingTransaction != null)
            {
                // Update the transaction
                _conn.Update(transaction);

                // Optional: Update tags if needed
                // First, delete the old associations
                _conn.Table<TransactionTag>()
                    .Where(tt => tt.TransactionId == transaction.Id)
                    .Delete();

                // Add new tags if any
                if (transaction.Tags != null && transaction.Tags.Count > 0)
                {
                    foreach (var tag in transaction.Tags)
                    {
                        var transactionTag = new TransactionTag
                        {
                            TransactionId = transaction.Id,
                            TagId = tag.Id
                        };
                        _conn.Insert(transactionTag);
                    }
                }
            }
            else
            {
                throw new Exception("Transaction not found for update.");
            }
        }


    }
}
