using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HxFood.Api.Infrastructure.Constants;
using HxFood.Api.Infrastructure.Extensions;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Options;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Models.Requests.Product;
using HxFood.Api.Models.Responses;
using HxFood.Api.Models.Responses.Category;
using HxFood.Api.Models.Responses.Product;
using HxFood.Api.Services.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace HxFood.Api.Services.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Category> _categories;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ProductManager> _logger;
        private readonly ICategoryService _categoryService;

        public ProductManager(IOptions<MongoDbSettings> mongoDbSettings, IMapper mapper, ILogger<ProductManager> logger, IDistributedCache cache, ICategoryService categoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _categoryService = categoryService;
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DbName);
            _products = database.GetCollection<Product>("products");
            _categories = database.GetCollection<Category>("categories");
        }
        
        public async Task<BaseResponse<List<ProductResponse>>> GetAsync()
        {
            var response = new BaseResponse<List<ProductResponse>>();
            
            var products = await (from p in _products.AsQueryable()
                join c in _categories.AsQueryable() on p.CategoryId equals c.Id
                select new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Currency = p.Currency,
                    Price = p.Price,
                    Category = new CategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }
                }).ToListAsync();
            
            
            response.Data = products;

            return response;
        }

        public async Task<BaseResponse<ProductResponse>> GetAsync(string id)
        {
            var response = new BaseResponse<ProductResponse>();
            ProductResponse productResponse;
            string cacheKey = $"{CacheConstants.ProductDetailKey}/{id}";
            
            var productFromCache = await _cache.GetAsync(cacheKey);

            if (productFromCache != null)
            {
                productResponse = JsonConvert.DeserializeObject<ProductResponse>(Encoding.UTF8.GetString(productFromCache));
                response.Data = productResponse;
                return response;
            }
            
            var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (product == null)
            {
                response.AddError("Product not found.");
                return response;
            }

            var category = await _categoryService.GetAsync(product.CategoryId);
            
            productResponse = product.ToProductResponse(category.Data);
            response.Data = productResponse;
            
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(5));
            await _cache.SetAsync(cacheKey, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(productResponse)), options);
            
            return response;
        }

        public async Task<BaseResponse<ProductResponse>> CreateAsync(ProductAddRequest request)
        {
            var response = new BaseResponse<ProductResponse>();
            
            var product = _mapper.Map<Product>(request);

            var category = await _categoryService.GetAsync(request.CategoryId);
            if (category.HasError)
            {
                response.AddError("Category not found.");
                return response;
            }
            
            await _products.InsertOneAsync(product);
            response.Data = product.ToProductResponse(category.Data);
            
            return response;
        }

        public async Task<BaseResponse<bool>> UpdateAsync(string id, ProductUpdateRequest request)
        {
            var response = new BaseResponse<bool>();

            var product = _mapper.Map<Product>(request);
            
            var category = await _categoryService.GetAsync(request.CategoryId);
            if (category.HasError)
            {
                response.AddError("Category not found.");
                return response;
            }
            
            var replaceOneResult = await _products.ReplaceOneAsync(p => p.Id == id, product);
            
            if (!replaceOneResult.IsAcknowledged)
            {
                response.AddError("Product could not be updated.");
                return response;
            }
            
            response.Data = replaceOneResult.IsAcknowledged;
            return response;
        }

        public async Task<BaseResponse<bool>> RemoveAsync(string id)
        {
            var response = new BaseResponse<bool>();
            
            var deleteOneResult =  await _products.DeleteOneAsync(p => p.Id == id);
            
            if (deleteOneResult.DeletedCount == 0)
            {
                response.AddError("Product could not be deleted.");
                return response;
            }

            response.Data = deleteOneResult.IsAcknowledged;
            return response;
        }

        public async Task<BaseResponse<List<ProductResponse>>> GetProductsByNameAsync(string name)
        {
            var response = new BaseResponse<List<ProductResponse>>();
            
            var products = await (from p in _products.AsQueryable()
                join c in _categories.AsQueryable() on p.CategoryId equals c.Id
                where p.Name.ToLowerInvariant().Contains(name.ToLowerInvariant())
                select new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Currency = p.Currency,
                    Price = p.Price,
                    Category = new CategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description
                    }
                }).ToListAsync();
            
            
            response.Data = products;

            return response;
        }
    }
}