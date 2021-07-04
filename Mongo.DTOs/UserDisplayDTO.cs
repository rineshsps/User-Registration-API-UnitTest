using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.DTOs
{
    public class UserDisplayDTO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
    }
}
