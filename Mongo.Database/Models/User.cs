using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Database.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }//Email
        public string Password { get; set; }
    }
}
