using HxFood.Api.Models.Responses.Category;

namespace HxFood.Api.Models.Responses.Product
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public string Currency { get; set; }

        public CategoryResponse Category { get; set; }
    }
}