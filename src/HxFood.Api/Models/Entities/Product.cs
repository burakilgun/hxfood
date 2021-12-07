using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HxFood.Api.Models.Entities
{
    public class Product : BaseEntity
    {
        [BsonElement("Name")]
        [Required]
        public string Name { get; set; }
        
        [BsonElement("Description")]
        public string Description { get; set; }
        
        [BsonElement("CategoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        
        [BsonElement("Price")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#,0}")]
        public decimal Price { get; set; }
        
        [BsonElement("Currency")]
        [Required]
        public string Currency { get; set; }
    }
}