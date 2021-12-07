using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace HxFood.Api.Models.Entities
{
    public class Category : BaseEntity
    {
        [BsonElement("Name")]
        [Required]
        public string Name { get; set; }
        
        [BsonElement("Description")]
        public string Description { get; set; }
    }
}