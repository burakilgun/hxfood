using System.Collections.Generic;
using System.Threading.Tasks;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Models.Responses;

namespace HxFood.Api.Services.Abstract
{
    public interface ICategoryService
    {
        Task<BaseResponse<List<Category>>> GetAsync();
        Task<BaseResponse<Category>> GetAsync(string id);
        Task<BaseResponse<Category>> CreateAsync(CategoryAddRequest request);
        Task<BaseResponse<bool>> UpdateAsync(string id, CategoryUpdateRequest request);
        Task<BaseResponse<bool>> RemoveAsync(string id);
        Task<BaseResponse<List<Category>>> GetCategoriesByNameAsync(string name);
    }
}