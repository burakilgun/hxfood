using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HxFood.Api.Models.Entities
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}