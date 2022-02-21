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
        public static void SeedData(IMongoDatabase database)
        {
            var bookCollection = database.GetCollection<Book>(nameof(Book));

            // delete all
            bookCollection.DeleteMany(_ => true);

            // re-create books
            bookCollection.InsertMany(BookCollection.Books);
        }
    }
}
