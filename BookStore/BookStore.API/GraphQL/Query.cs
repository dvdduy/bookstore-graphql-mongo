using BookStore.API.GraphQL.Types;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using HotChocolate;
using HotChocolate.Data;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.GraphQL
{
    public class Query
    {
        public async Task<IEnumerable<Book>> GetBooksAsync([Service] IBookRepository bookRepository)
        {
            try
            {
                return await bookRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to retrieve books: {ex.Message}");
            }
        }

        public async Task<PagedBooksResult> GetPagedBooksAsync(
            [Service] IBookRepository bookRepository,
            int page = 1,
            int pageSize = 10)
        {
            // Validation
            if (page < 1)
            {
                throw new GraphQLException("Page must be greater than 0");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new GraphQLException("Page size must be between 1 and 100");
            }

            try
            {
                var (books, totalCount) = await bookRepository.GetPagedAsync(page, pageSize);
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new PagedBooksResult
                {
                    Books = books,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasNextPage = page < totalPages,
                    HasPreviousPage = page > 1
                };
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to retrieve paged books: {ex.Message}");
            }
        }

        public async Task<Book> GetBookAsync([Service] IBookRepository bookRepository, string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new GraphQLException("Book ID is required and cannot be empty");
            }

            if (!ObjectId.TryParse(id, out _))
            {
                throw new GraphQLException($"Invalid book ID format: '{id}'. Expected a valid MongoDB ObjectId");
            }

            try
            {
                var book = await bookRepository.GetByIdAsync(id);
                
                if (book == null)
                {
                    throw new GraphQLException($"Book with ID '{id}' was not found");
                }

                return book;
            }
            catch (GraphQLException)
            {
                // Re-throw GraphQL exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to retrieve book: {ex.Message}");
            }
        }
    }
}
