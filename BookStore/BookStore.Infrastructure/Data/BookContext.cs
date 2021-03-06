using BookStore.Infrastructure.Configurations;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Data
{
    public class BookContext : IBookContext
    {
        private readonly IMongoDatabase _mongoDb;

        public BookContext(MongoDbConfiguration mongoDbConfig)
        {
            _mongoDb = new MongoClient(mongoDbConfig.ConnectionString).GetDatabase(mongoDbConfig.Database);
            BookContextSeed.SeedData(_mongoDb);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _mongoDb.GetCollection<T>(name);
        }
    }
}
