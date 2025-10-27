using BookStore.Core.Entities;
using BookStore.Core.Repositories;
using BookStore.Infrastructure.Data;
using BookStore.Infrastructure.Repositories;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.UnitTests.Repositories
{
    public class BookRepositoryTests
    {
        private readonly Mock<IBookContext> _mockContext;
        private readonly Mock<IMongoCollection<Book>> _mockCollection;
        private readonly BookRepository _repository;

        public BookRepositoryTests()
        {
            _mockContext = new Mock<IBookContext>();
            _mockCollection = new Mock<IMongoCollection<Book>>();
            
            _mockContext.Setup(c => c.GetCollection<Book>(It.IsAny<string>()))
                .Returns(_mockCollection.Object);
            
            _repository = new BookRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllAsync__ReturnsAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                CreateTestBook("Book 1"),
                CreateTestBook("Book 2"),
                CreateTestBook("Book 3")
            };

            var mockCursor = new Mock<IAsyncCursor<Book>>();
            mockCursor.Setup(c => c.Current).Returns(books);
            mockCursor
                .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<FindOptions<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetPagedAsync__Page2_PageSize5__ReturnsCorrectPage_TotalCount()
        {
            // Arrange
            var allBooks = new List<Book>();
            for (int i = 1; i <= 15; i++)
            {
                allBooks.Add(CreateTestBook($"Book {i}"));
            }

            var page = 2;
            var pageSize = 5;
            var pagedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var mockCursor = new Mock<IAsyncCursor<Book>>();
            mockCursor.Setup(c => c.Current).Returns(pagedBooks);
            mockCursor
                .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.CountDocumentsAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<CountOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(allBooks.Count);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<FindOptions<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var (books, totalCount) = await _repository.GetPagedAsync(page, pageSize);

            // Assert
            books.Should().NotBeNull();
            books.Should().HaveCount(5);
            totalCount.Should().Be(15);
            books.First().Title.Should().Be("Book 6");
        }

        [Fact]
        public async Task GetByIdAsync__ValidId__ReturnsBook()
        {
            // Arrange
            var expectedBook = CreateTestBook("Expected Book");
            var bookId = expectedBook.Id;

            var mockCursor = new Mock<IAsyncCursor<Book>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Book> { expectedBook });
            mockCursor
                .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<FindOptions<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetByIdAsync(bookId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Expected Book");
        }

        [Fact]
        public async Task GetByIdAsync__InvalidId__ReturnsNull()
        {
            // Arrange
            var mockCursor = new Mock<IAsyncCursor<Book>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Book>());
            mockCursor
                .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(false);
            mockCursor
                .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<FindOptions<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetByIdAsync("invalid-id");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync__NewBook__SetsTimestamps_InsertsBook()
        {
            // Arrange
            var book = CreateTestBook("New Book");
            var beforeCreate = DateTime.UtcNow;

            _mockCollection.Setup(c => c.InsertOneAsync(
                It.IsAny<Book>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _repository.CreateAsync(book);

            // Assert
            result.Should().NotBeNull();
            result.CreatedAt.Should().BeCloseTo(beforeCreate, TimeSpan.FromSeconds(1));
            result.UpdatedAt.Should().BeCloseTo(beforeCreate, TimeSpan.FromSeconds(1));
            _mockCollection.Verify(c => c.InsertOneAsync(
                It.Is<Book>(b => b.Id == book.Id),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync__ExistingBook__UpdatesBook_ReturnsTrue()
        {
            // Arrange
            var book = CreateTestBook("Updated Book");
            var bookId = book.Id;
            var beforeUpdate = DateTime.UtcNow;

            var mockResult = new Mock<ReplaceOneResult>();
            mockResult.Setup(r => r.ModifiedCount).Returns(1);

            _mockCollection.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<Book>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResult.Object);

            // Act
            var result = await _repository.UpdateAsync(bookId, book);

            // Assert
            result.Should().BeTrue();
            book.UpdatedAt.Should().BeCloseTo(beforeUpdate, TimeSpan.FromSeconds(1));
            _mockCollection.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.Is<Book>(b => b.Id == bookId),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync__NonExistingBook__ReturnsFalse()
        {
            // Arrange
            var book = CreateTestBook("Non-existing Book");
            var bookId = "non-existing-id";

            var mockResult = new Mock<ReplaceOneResult>();
            mockResult.Setup(r => r.ModifiedCount).Returns(0);

            _mockCollection.Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<Book>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResult.Object);

            // Act
            var result = await _repository.UpdateAsync(bookId, book);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync__ExistingBook__ReturnsTrue()
        {
            // Arrange
            var bookId = "existing-book-id";

            var mockResult = new Mock<DeleteResult>();
            mockResult.Setup(r => r.DeletedCount).Returns(1);

            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResult.Object);

            // Act
            var result = await _repository.DeleteAsync(bookId);

            // Assert
            result.Should().BeTrue();
            _mockCollection.Verify(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync__NonExistingBook__ReturnsFalse()
        {
            // Arrange
            var bookId = "non-existing-book-id";

            var mockResult = new Mock<DeleteResult>();
            mockResult.Setup(r => r.DeletedCount).Returns(0);

            _mockCollection.Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Book>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResult.Object);

            // Act
            var result = await _repository.DeleteAsync(bookId);

            // Assert
            result.Should().BeFalse();
        }

        private Book CreateTestBook(string title)
        {
            return new Book
            {
                Title = title,
                Description = $"Description for {title}",
                Publisher = "Test Publisher",
                PublishedDate = DateTime.UtcNow,
                Length = 300,
                ImageUrl = "https://example.com/image.jpg",
                Authors = new List<Author>
                {
                    new Author { Id = "author-1", Name = "Test Author" }
                },
                Reviews = new List<Review>()
            };
        }
    }
}

