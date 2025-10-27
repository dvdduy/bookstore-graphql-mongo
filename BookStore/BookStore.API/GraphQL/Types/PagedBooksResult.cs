using BookStore.Core.Entities;
using System.Collections.Generic;

namespace BookStore.API.GraphQL.Types
{
    public class PagedBooksResult
    {
        public IEnumerable<Book> Books { get; set; }
        public long TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}

