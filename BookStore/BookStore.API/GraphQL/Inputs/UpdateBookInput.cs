using System;
using System.Collections.Generic;

namespace BookStore.API.GraphQL.Inputs
{
    public class UpdateBookInput
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Publisher { get; set; }
        public int Length { get; set; }
        public List<AuthorInput> Authors { get; set; }
    }
}

