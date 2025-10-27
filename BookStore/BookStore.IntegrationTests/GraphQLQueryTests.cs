using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BookStore.IntegrationTests
{
    public class GraphQLQueryTests : IClassFixture<GraphQLTestFixture>
    {
        private readonly GraphQLTestFixture _fixture;

        public GraphQLQueryTests(GraphQLTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetBooksAsync__NoParameters__ReturnsBooksList_OkStatus()
        {
            // Arrange
            var query = @"
                query {
                    books {
                        id
                        title
                        description
                        publisher
                        length
                        authors {
                            name
                        }
                    }
                }";

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            
            var jsonDoc = JsonDocument.Parse(content);
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("books", out var books).Should().BeTrue();
            books.ValueKind.Should().Be(JsonValueKind.Array);
        }

        [Fact]
        public async Task GetPagedBooksAsync__Page1_PageSize5__ReturnsPagedResults_CorrectMetadata()
        {
            // Arrange
            var query = @"
                query GetPagedBooks($page: Int!, $pageSize: Int!) {
                    pagedBooks(page: $page, pageSize: $pageSize) {
                        books {
                            id
                            title
                            publisher
                        }
                        totalCount
                        page
                        pageSize
                        totalPages
                        hasNextPage
                        hasPreviousPage
                    }
                }";

            var variables = new { page = 1, pageSize = 5 };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("pagedBooks", out var pagedBooks).Should().BeTrue();
            
            pagedBooks.TryGetProperty("page", out var page).Should().BeTrue();
            page.GetInt32().Should().Be(1);
            
            pagedBooks.TryGetProperty("pageSize", out var pageSize).Should().BeTrue();
            pageSize.GetInt32().Should().Be(5);
            
            pagedBooks.TryGetProperty("books", out var books).Should().BeTrue();
            books.ValueKind.Should().Be(JsonValueKind.Array);
            books.GetArrayLength().Should().BeLessThanOrEqualTo(5);
        }

        [Fact]
        public async Task GetBookAsync__ValidId__ReturnsBookDetails_OkStatus()
        {
            // Arrange
            // First, get a book ID from the books query
            var booksQuery = "query { books { id } }";
            var booksResponse = await _fixture.ExecuteGraphQLQuery(booksQuery);
            var booksContent = await booksResponse.Content.ReadAsStringAsync();
            var booksDoc = JsonDocument.Parse(booksContent);
            
            var bookId = booksDoc.RootElement
                .GetProperty("data")
                .GetProperty("books")[0]
                .GetProperty("id")
                .GetString();

            var query = @"
                query GetBook($id: String!) {
                    book(id: $id) {
                        id
                        title
                        description
                        publisher
                        publishedDate
                        length
                        authors {
                            name
                        }
                        reviews {
                            rating
                            title
                            description
                        }
                        averageReview
                    }
                }";

            var variables = new { id = bookId };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("data", out var data).Should().BeTrue();
            data.TryGetProperty("book", out var book).Should().BeTrue();
            
            book.TryGetProperty("id", out var id).Should().BeTrue();
            id.GetString().Should().Be(bookId);
            
            book.TryGetProperty("title", out var title).Should().BeTrue();
            title.GetString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetBookAsync__InvalidId__ReturnsError()
        {
            // Arrange
            var query = @"
                query GetBook($id: String!) {
                    book(id: $id) {
                        id
                        title
                    }
                }";

            var variables = new { id = "invalid-id-format" };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
            errors.ValueKind.Should().Be(JsonValueKind.Array);
            errors.GetArrayLength().Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetBookAsync__EmptyId__ReturnsValidationError()
        {
            // Arrange
            var query = @"
                query GetBook($id: String!) {
                    book(id: $id) {
                        id
                        title
                    }
                }";

            var variables = new { id = "" };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }

        [Fact]
        public async Task GetPagedBooksAsync__InvalidPage__ReturnsValidationError()
        {
            // Arrange
            var query = @"
                query GetPagedBooks($page: Int!, $pageSize: Int!) {
                    pagedBooks(page: $page, pageSize: $pageSize) {
                        books { id }
                    }
                }";

            var variables = new { page = 0, pageSize = 10 };

            // Act
            var response = await _fixture.ExecuteGraphQLQuery(query, variables);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            
            jsonDoc.RootElement.TryGetProperty("errors", out var errors).Should().BeTrue();
        }
    }
}

