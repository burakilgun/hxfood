using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Options;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Models.Responses;
using HxFood.Api.Services.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HxFood.Api.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IMongoCollection<Category> _categories;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryManager> _logger;

        public CategoryManager(IOptions<MongoDbSettings> mongoDbSettings, IMapper mapper, ILogger<CategoryManager> logger)
        {
            _mapper = mapper;
            _logger = logger;
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DbName);
            _categories = database.GetCollection<Category>("categories");
        }
        
        public async Task<BaseResponse<List<Category>>> GetAsync()
        {
            var response = new BaseResponse<List<Category>>();
            
            var categories = await _categories.Find(p => true).ToListAsync();
            response.Data = categories;

            return response;
        }

        public async Task<BaseResponse<Category>> GetAsync(string id)
        {
            var response = new BaseResponse<Category>();

            var category = await _categories.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                response.AddError("Category not found.");
                return response;
            }

            response.Data = category;
            return response;
        }

        public async Task<BaseResponse<Category>> CreateAsync(CategoryAddRequest request)
        {
            var response = new BaseResponse<Category>();
            
            var category = _mapper.Map<Category>(request);
            
            await _categories.InsertOneAsync(category);
            response.Data = category;
            
            return response;
        }

        public async Task<BaseResponse<bool>> UpdateAsync(string id, CategoryUpdateRequest request)
        {
            var response = new BaseResponse<bool>();

            var category = _mapper.Map<Category>(request);
            var replaceOneResult = await _categories.ReplaceOneAsync(p => p.Id == id, category);
            
            if (!replaceOneResult.IsAcknowledged)
            {
                response.AddError("Category could not be updated.");
                return response;
            }
            
            response.Data = replaceOneResult.IsAcknowledged;
            return response;
        }

        public async Task<BaseResponse<bool>> RemoveAsync(string id)
        {
            var response = new BaseResponse<bool>();
            
            var deleteOneResult =  await _categories.DeleteOneAsync(p => p.Id == id);
            
            if (deleteOneResult.DeletedCount == 0)
            {
                response.AddError("Category could not be deleted.");
                return response;
            }

            response.Data = deleteOneResult.IsAcknowledged;
            return response;
        }

        public async Task<BaseResponse<List<Category>>> GetCategoriesByNameAsync(string name)
        {
            var response = new BaseResponse<List<Category>>();
            
            var categories = await _categories.Find(p => p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())).ToListAsync();
            response.Data = categories;

            return response;
        }
    }
}