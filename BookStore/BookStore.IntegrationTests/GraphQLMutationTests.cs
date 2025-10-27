using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BookStore.IntegrationTests
{
    public class GraphQLMutationTests : IClassFixture<GraphQLTestFixture>
    {
        private readonly GraphQLTestFixture _fixture;

        public GraphQLMutationTests(GraphQLTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddBookAsync__ValidInput__CreatesBook_ReturnsNewBook()
        {
            // Arrange
            var mutation = @"
                mutation AddBook($input: AddBookInput!) {
                    addBook(input: $input) {
                        id
                        title
                        description
                        publisher
                        publishedDate
                        length
                        authors {
                            name
                        }
                    }
                }";

            var input = new
            {
                title = "Integration Test Book",
                description = "A book created during integration testing",
                imageUrl = "https://example.com/test-book.jpg",
                publisher = "Test Publisher",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 350,
                authors = new[] { new { name = "Test Author" } }
            };

            var variables = new { input };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(mutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("addBook", out var addedBook).Should().BeTrue();
            
            addedBook.TryGetProperty("id", out var id).Should().BeTrue();
            id.GetString().Should().NotBeNullOrEmpty();
            
            addedBook.TryGetProperty("title", out var title).Should().BeTrue();
            title.GetString().Should().Be("Integration Test Book");
            
            addedBook.TryGetProperty("authors", out var authors).Should().BeTrue();
            authors.ValueKind.Should().Be(JsonValueKind.Array);
            authors.GetArrayLength().Should().Be(1);
        }

        [Fact]
        public async Task AddBookAsync__MissingTitle__ReturnsValidationError()
        {
            // Arrange
            var mutation = @"
                mutation AddBook($input: AddBookInput!) {
                    addBook(input: $input) {
                        id
                        title
                    }
                }";

            var input = new
            {
                title = "",
                description = "Test description",
                imageUrl = "",
                publisher = "Test Publisher",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 300,
                authors = new[] { new { name = "Test Author" } }
            };

            var variables = new { input };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(mutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }

        [Fact]
        public async Task UpdateBookAsync__ValidInput__UpdatesBook_ReturnsUpdatedBook()
        {
            // Arrange
            // First, create a book to update
            var createMutation = @"
                mutation AddBook($input: AddBookInput!) {
                    addBook(input: $input) {
                        id
                        title
                    }
                }";

            var createInput = new
            {
                title = "Book to Update",
                description = "Original description",
                imageUrl = "",
                publisher = "Original Publisher",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 200,
                authors = new[] { new { name = "Original Author" } }
            };

            var createResponse = await _fixture.ExecuteGraphQLQuery(createMutation, new { input = createInput });
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createDoc = JsonDocument.Parse(createContent);
            
            var bookId = createDoc.RootElement
                .GetProperty("data")
                .GetProperty("addBook")
                .GetProperty("id")
                .GetString();

            // Now update the book
            var updateMutation = @"
                mutation UpdateBook($input: UpdateBookInput!) {
                    updateBook(input: $input) {
                        id
                        title
                        description
                        publisher
                        authors {
                            name
                        }
                    }
                }";

            var updateInput = new
            {
                id = bookId,
                title = "Updated Book Title",
                description = "Updated description",
                imageUrl = "",
                publisher = "Updated Publisher",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 250,
                authors = new[] { new { name = "Updated Author" } }
            };

            var updateVariables = new { input = updateInput };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(updateMutation, updateVariables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("updateBook", out var updatedBook).Should().BeTrue();
            
            updatedBook.TryGetProperty("title", out var title).Should().BeTrue();
            title.GetString().Should().Be("Updated Book Title");
            
            updatedBook.TryGetProperty("publisher", out var publisher).Should().BeTrue();
            publisher.GetString().Should().Be("Updated Publisher");
        }

        [Fact]
        public async Task UpdateBookAsync__NonExistentId__ReturnsError()
        {
            // Arrange
            var mutation = @"
                mutation UpdateBook($input: UpdateBookInput!) {
                    updateBook(input: $input) {
                        id
                        title
                    }
                }";

            var input = new
            {
                id = "507f1f77bcf86cd799439011", // Valid ObjectId format but doesn't exist
                title = "Non-existent Book",
                description = "Test",
                imageUrl = "",
                publisher = "Test",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 100,
                authors = new[] { new { name = "Test" } }
            };

            var variables = new { input };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(mutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }

        [Fact]
        public async Task DeleteBookAsync__ValidId__DeletesBook_ReturnsTrue()
        {
            // Arrange
            // First, create a book to delete
            var createMutation = @"
                mutation AddBook($input: AddBookInput!) {
                    addBook(input: $input) {
                        id
                    }
                }";

            var createInput = new
            {
                title = "Book to Delete",
                description = "This book will be deleted",
                imageUrl = "",
                publisher = "Test Publisher",
                publishedDate = "2024-01-01T00:00:00Z",
                length = 150,
                authors = new[] { new { name = "Test Author" } }
            };

            var createResponse = await _fixture.ExecuteGraphQLQuery(createMutation, new { input = createInput });
            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createDoc = JsonDocument.Parse(createContent);
            
            var bookId = createDoc.RootElement
                .GetProperty("data")
                .GetProperty("addBook")
                .GetProperty("id")
                .GetString();

            // Now delete the book
            var deleteMutation = @"
                mutation DeleteBook($id: String!) {
                    deleteBook(id: $id)
                }";

            var variables = new { id = bookId };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(deleteMutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("deleteBook", out var result).Should().BeTrue();
            result.GetBoolean().Should().BeTrue();
        }

        [Fact]
        public async Task DeleteBookAsync__NonExistentId__ReturnsError()
        {
            // Arrange
            var mutation = @"
                mutation DeleteBook($id: String!) {
                    deleteBook(id: $id)
                }";

            var variables = new { id = "507f1f77bcf86cd799439011" }; // Valid format but doesn't exist

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(mutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }

        [Fact]
        public async Task DeleteBookAsync__InvalidIdFormat__ReturnsValidationError()
        {
            // Arrange
            var mutation = @"
                mutation DeleteBook($id: String!) {
                    deleteBook(id: $id)
                }";

            var variables = new { id = "invalid-id" };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(mutation, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }
    }
}

