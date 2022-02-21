using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.GraphQL
{
    public class Query
    {
        public Task<IEnumerable<Book>> GetBooksAsync([Service] IBookRepository bookRepository) => bookRepository.GetAllAsync();

        public Task<Book> GetBookAsync([Service] IBookRepository bookRepository, string id) => bookRepository.GetByIdAsync(id);
    }
}
