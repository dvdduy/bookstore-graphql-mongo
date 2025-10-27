using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMongoCollection<Book> _bookCollection;

        public BookRepository(IBookContext bookContext)
        {
            _bookCollection = bookContext.GetCollection<Book>(nameof(Book));
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _bookCollection.Find(_ => true).ToListAsync();
        }

        public async Task<(IEnumerable<Book> Books, long TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var totalCount = await _bookCollection.CountDocumentsAsync(_ => true);
            var skip = (page - 1) * pageSize;
            
            var books = await _bookCollection
                .Find(_ => true)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }

        public async Task<Book> GetByIdAsync(string id)
        {
            return await _bookCollection.Find(Builders<Book>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = DateTime.UtcNow;
            await _bookCollection.InsertOneAsync(book);
            return book;
        }

        public async Task<bool> UpdateAsync(string id, Book book)
        {
            book.UpdatedAt = DateTime.UtcNow;
            var result = await _bookCollection.ReplaceOneAsync(
                Builders<Book>.Filter.Eq(x => x.Id, id),
                book
            );
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _bookCollection.DeleteOneAsync(
                Builders<Book>.Filter.Eq(x => x.Id, id)
            );
            return result.DeletedCount > 0;
        }
    }
}
