using MongoDB.Bson;

namespace BookStore.Core.Entities
{
    public class Review : IHasId
    {
        public string Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public Review()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
