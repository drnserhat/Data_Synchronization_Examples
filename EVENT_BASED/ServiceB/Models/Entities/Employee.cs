using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ServiceB.Models.Entities
{
    public class Employee
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement(Order = 0)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 1)]
        public string PersonId { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 2)]
        public string Name { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 3)]
        public string Department { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [BsonElement(Order = 4)]
        public decimal Salary { get; set; }
    }
}
