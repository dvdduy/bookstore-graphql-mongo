using BookStore.Core.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BookStore.Tests.Entities
{
    public class BookTests
    {
        [Fact]
        public void AverageReview__NoReviews__ReturnsNull()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>()
            };

            // Act
            var average = book.AverageReview;

            // Assert
            average.Should().BeNull();
        }

        [Fact]
        public void AverageReview__OneReview__ReturnsExactRating()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>
                {
                    new Review { Id = "1", Rating = 4, Title = "Good", Description = "Nice book" }
                }
            };

            // Act
            var average = book.AverageReview;

            // Assert
            average.Should().Be(4.0);
        }

        [Fact]
        public void AverageReview__MultipleReviews__ReturnsCorrectAverage()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>
                {
                    new Review { Id = "1", Rating = 5, Title = "Excellent", Description = "Amazing" },
                    new Review { Id = "2", Rating = 4, Title = "Good", Description = "Nice" },
                    new Review { Id = "3", Rating = 3, Title = "OK", Description = "Average" }
                }
            };

            // Act
            var average = book.AverageReview;

            // Assert
            average.Should().Be(4.0); // (5 + 4 + 3) / 3 = 4.0
        }

        [Fact]
        public void AverageReview__MixedRatings__RoundsToOneDecimal()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>
                {
                    new Review { Id = "1", Rating = 5, Title = "Great", Description = "Loved it" },
                    new Review { Id = "2", Rating = 4, Title = "Good", Description = "Enjoyed it" },
                    new Review { Id = "3", Rating = 4, Title = "Decent", Description = "OK read" }
                }
            };

            // Act
            var average = book.AverageReview;

            // Assert
            // (5 + 4 + 4) / 3 = 4.333... should round to 4.3
            average.Should().Be(4.3);
        }

        [Fact]
        public void BookConstructor__NewBook__GeneratesId_SetsTimestamps()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var book = new Book
            {
                Title = "New Book",
                Publisher = "Test Publisher",
                PublishedDate = DateTime.UtcNow,
                Length = 300
            };

            var afterCreation = DateTime.UtcNow;

            // Assert
            book.Id.Should().NotBeNullOrEmpty();
            book.CreatedAt.Should().BeOnOrAfter(beforeCreation);
            book.CreatedAt.Should().BeOnOrBefore(afterCreation);
            book.UpdatedAt.Should().BeOnOrAfter(beforeCreation);
            book.UpdatedAt.Should().BeOnOrBefore(afterCreation);
        }

        [Fact]
        public void BookConstructor__NewBook__InitializesEmptyReviewsList()
        {
            // Act
            var book = new Book
            {
                Title = "Test Book"
            };

            // Assert
            book.Reviews.Should().NotBeNull();
            book.Reviews.Should().BeEmpty();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void AverageReview__SingleRating__ReturnsExactRating(int rating)
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>
                {
                    new Review { Id = "1", Rating = rating, Title = "Review", Description = "Test" }
                }
            };

            // Act
            var average = book.AverageReview;

            // Assert
            average.Should().Be((double)rating);
        }

        [Fact]
        public void AverageReview__ExtremeRatings__CalculatesCorrectly()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Reviews = new List<Review>
                {
                    new Review { Id = "1", Rating = 5, Title = "Perfect", Description = "Best book ever" },
                    new Review { Id = "2", Rating = 1, Title = "Terrible", Description = "Worst book ever" }
                }
            };

            // Act
            var average = book.AverageReview;

            // Assert
            average.Should().Be(3.0); // (5 + 1) / 2 = 3.0
        }
    }
}

