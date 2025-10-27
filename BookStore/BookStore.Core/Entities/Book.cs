using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Core.Entities
{
    public class Book : EntityBase, IHasId
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Publisher { get; set; }
        public int Length { get; set; }
        public IList<Author> Authors { get; set; }
        public IList<Review> Reviews { get; set; } = new List<Review>();
        public double? AverageReview => Reviews.Any() ? Math.Round(Reviews.Select(x => x.Rating).Average(), 1) : null;
    }
}
