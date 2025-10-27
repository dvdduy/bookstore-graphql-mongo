using BookStore.Core.Entities;
using BookStore.Infrastructure.Configurations;
using Microsoft.Extensions.Hosting;
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

        public BookContext(IMongoClient mongoClient, MongoDbConfiguration mongoDbConfig, IHostEnvironment env)
        {
            _mongoDb = mongoClient.GetDatabase(mongoDbConfig.Database);
            
            // Create indexes
            CreateIndexes();
            
            // Seed data
            BookContextSeed.SeedData(_mongoDb, env.IsDevelopment());
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _mongoDb.GetCollection<T>(name);
        }

        private void CreateIndexes()
        {
            var bookCollection = _mongoDb.GetCollection<Book>(nameof(Book));

            // Create index on Title for text search
            var titleIndexModel = new CreateIndexModel<Book>(
                Builders<Book>.IndexKeys.Ascending(x => x.Title),
                new CreateIndexOptions { Name = "Title_Index" }
            );

            // Create index on Publisher
            var publisherIndexModel = new CreateIndexModel<Book>(
                Builders<Book>.IndexKeys.Ascending(x => x.Publisher),
                new CreateIndexOptions { Name = "Publisher_Index" }
            );

            // Create index on PublishedDate for sorting
            var publishedDateIndexModel = new CreateIndexModel<Book>(
                Builders<Book>.IndexKeys.Descending(x => x.PublishedDate),
                new CreateIndexOptions { Name = "PublishedDate_Index" }
            );

            // Create compound index for Title and Publisher
            var compoundIndexModel = new CreateIndexModel<Book>(
                Builders<Book>.IndexKeys
                    .Ascending(x => x.Title)
                    .Ascending(x => x.Publisher),
                new CreateIndexOptions { Name = "Title_Publisher_Index" }
            );

            bookCollection.Indexes.CreateMany(new[]
            {
                titleIndexModel,
                publisherIndexModel,
                publishedDateIndexModel,
                compoundIndexModel
            });
        }
    }
}
