using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ServiceA.Models.Entities
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement(Order = 0)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 1)]
        public string Name { get; set; }
    }
}
