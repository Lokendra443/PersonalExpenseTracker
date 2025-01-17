using PersonalExpenseTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalExpenseTracker.Services
{
    internal class TagService : ITagService
    {
        string dbPath = @"C:\Users\loken\Desktop\db.db3";
        private SQLiteConnection _conn;

        public TagService()
        {
            _conn = new SQLiteConnection(dbPath);  // Ensure connection is established
            _conn.CreateTable<Tag>();  // Create the Tag table if it doesn't exist
        }


        public List<Tag> GetAllTags()
        {
            var predefinedTags = new List<Tag>
            {
                new Tag { Id = 1, Name = "Food" },
                new Tag { Id = 2, Name = "Drinks" },
                new Tag { Id = 3, Name = "Clothes" },
                new Tag { Id = 4, Name = "Gadgets" },
                new Tag { Id = 5, Name = "Fuel" },
                new Tag { Id = 6, Name = "Rent" },
                new Tag { Id = 7, Name = "EMI" },
                new Tag { Id = 8, Name = "Party" }
            };

            var userTags = _conn.Table<Tag>().ToList();
            return predefinedTags.Concat(userTags).ToList();
        }


        public void AddCustomTag(string tagName)
        {
            if (_conn.Table<Tag>().Any(t => t.Name == tagName)) return;

            var newTag = new Tag { Name = tagName };
            _conn.Insert(newTag);
        }


        // Get all tags associated with a specific transaction
        public List<Tag> GetTagsByTransactionId(int transactionId)
        {
            var tagIds = _conn.Table<TransactionTag>()
                              .Where(tt => tt.TransactionId == transactionId)
                              .Select(tt => tt.TagId)
                              .ToList();

            return _conn.Table<Tag>()
                        .Where(t => tagIds.Contains(t.Id))
                        .ToList();
        }


    }
}
