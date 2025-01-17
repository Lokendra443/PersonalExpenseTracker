using PersonalExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    public interface ITransactionService
    {
        void AddTransaction(Transaction transaction, List<int> tagIds);  // Add transaction with selected tag IDs
        List<Transaction> GetAllTransactions();

        List<Transaction> GetFilteredTransactions(
            string searchText,
            string transactionType,
            int? tagId,
            DateTime? startDate,
            DateTime? endDate);


        List<Transaction> GetPendingDebts();

        bool UpdateDebtStatus(int transactionId, string newStatus);


        bool CheckSufficientBalanceForOutflow(decimal outflowAmount);

        void UpdateTransaction(Transaction transaction);


    }
}
