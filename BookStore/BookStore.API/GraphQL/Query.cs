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
