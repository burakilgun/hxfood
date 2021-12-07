using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Responses.Category;
using HxFood.Api.Models.Responses.Product;

namespace HxFood.Api.Infrastructure.Extensions
{
    public static class ProductExtension
    {
        public static ProductResponse ToProductResponse(this Product product, Category category)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Currency = product.Currency,
                Price = product.Price,
                Category = new CategoryResponse
                {
                    Id = category.Id,
                    Description = category.Description,
                    Name = category.Name
                }
            };
        }
    }
}