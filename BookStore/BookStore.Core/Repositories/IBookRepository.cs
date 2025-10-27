using BookStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<(IEnumerable<Book> Books, long TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<Book> GetByIdAsync(string id);
        Task<Book> CreateAsync(Book book);
        Task<bool> UpdateAsync(string id, Book book);
        Task<bool> DeleteAsync(string id);
    }
}
