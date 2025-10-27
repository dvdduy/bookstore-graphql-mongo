using BookStore.API.GraphQL.Inputs;
using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using HotChocolate;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.GraphQL
{
    public class Mutation
    {
        public async Task<Book> AddBookAsync(
            [Service] IBookRepository bookRepository,
            AddBookInput input)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new GraphQLException("Title is required");
            }

            if (input.Length <= 0)
            {
                throw new GraphQLException("Length must be greater than 0");
            }

            if (input.Authors == null || !input.Authors.Any())
            {
                throw new GraphQLException("At least one author is required");
            }

            try
            {
                var book = new Book
                {
                    Title = input.Title,
                    ImageUrl = input.ImageUrl,
                    Description = input.Description,
                    PublishedDate = input.PublishedDate,
                    Publisher = input.Publisher,
                    Length = input.Length,
                    Authors = input.Authors.Select(a => new Author
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        Name = a.Name
                    }).ToList()
                };

                return await bookRepository.CreateAsync(book);
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to create book: {ex.Message}");
            }
        }

        public async Task<Book> UpdateBookAsync(
            [Service] IBookRepository bookRepository,
            UpdateBookInput input)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new GraphQLException("Book ID is required");
            }

            if (!ObjectId.TryParse(input.Id, out _))
            {
                throw new GraphQLException($"Invalid book ID format: '{input.Id}'");
            }

            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new GraphQLException("Title is required");
            }

            if (input.Length <= 0)
            {
                throw new GraphQLException("Length must be greater than 0");
            }

            if (input.Authors == null || !input.Authors.Any())
            {
                throw new GraphQLException("At least one author is required");
            }

            try
            {
                // Check if book exists
                var existingBook = await bookRepository.GetByIdAsync(input.Id);
                if (existingBook == null)
                {
                    throw new GraphQLException($"Book with ID '{input.Id}' was not found");
                }

                // Update book properties
                existingBook.Title = input.Title;
                existingBook.ImageUrl = input.ImageUrl;
                existingBook.Description = input.Description;
                existingBook.PublishedDate = input.PublishedDate;
                existingBook.Publisher = input.Publisher;
                existingBook.Length = input.Length;
                existingBook.Authors = input.Authors.Select(a => new Author
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = a.Name
                }).ToList();

                var updated = await bookRepository.UpdateAsync(input.Id, existingBook);
                if (!updated)
                {
                    throw new GraphQLException($"Failed to update book with ID '{input.Id}'");
                }

                return existingBook;
            }
            catch (GraphQLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to update book: {ex.Message}");
            }
        }

        public async Task<bool> DeleteBookAsync(
            [Service] IBookRepository bookRepository,
            string id)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new GraphQLException("Book ID is required");
            }

            if (!ObjectId.TryParse(id, out _))
            {
                throw new GraphQLException($"Invalid book ID format: '{id}'");
            }

            try
            {
                // Check if book exists
                var existingBook = await bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                {
                    throw new GraphQLException($"Book with ID '{id}' was not found");
                }

                var deleted = await bookRepository.DeleteAsync(id);
                if (!deleted)
                {
                    throw new GraphQLException($"Failed to delete book with ID '{id}'");
                }

                return true;
            }
            catch (GraphQLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to delete book: {ex.Message}");
            }
        }
    }
}

