using System.Collections.Generic;
using System.Threading.Tasks;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Requests.Product;
using HxFood.Api.Models.Responses;
using HxFood.Api.Models.Responses.Product;

namespace HxFood.Api.Services.Abstract
{
    public interface IProductService
    {
        Task<BaseResponse<List<ProductResponse>>> GetAsync();
        Task<BaseResponse<ProductResponse>> GetAsync(string id);
        Task<BaseResponse<ProductResponse>> CreateAsync(ProductAddRequest request);
        Task<BaseResponse<bool>> UpdateAsync(string id, ProductUpdateRequest request);
        Task<BaseResponse<bool>> RemoveAsync(string id);
        Task<BaseResponse<List<ProductResponse>>> GetProductsByNameAsync(string name);
    }
}