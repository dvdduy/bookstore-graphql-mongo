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

        public async Task<Book> GetByIdAsync(string id)
        {
            return await _bookCollection.Find(Builders<Book>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync();
        }
    }
}
