using BookStore.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Data
{
    internal class BookContextSeed
    {
        public static void SeedData(IMongoDatabase database, bool isDevelopment)
        {
            // Only seed in Development environment
            if (!isDevelopment)
            {
                return;
            }

            var bookCollection = database.GetCollection<Book>(nameof(Book));

            // Only seed if the collection is empty
            var existingCount = bookCollection.CountDocuments(_ => true);
            if (existingCount == 0)
            {
                bookCollection.InsertMany(BookCollection.Books);
            }
        }
    }
}
