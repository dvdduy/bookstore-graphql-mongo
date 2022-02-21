using MongoDB.Bson;

namespace BookStore.Core.Entities
{
    public class Author : IHasId
    {        
        public string Id { get; set; }
        public string Name { get; set; }

        public Author()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}
