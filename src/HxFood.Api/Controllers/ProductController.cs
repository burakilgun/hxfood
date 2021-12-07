using System.Collections.Generic;
using System.Threading.Tasks;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Requests.Product;
using HxFood.Api.Models.Responses.Product;
using HxFood.Api.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HxFood.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var response = await _productService.GetAsync();
            return Ok(response.Data);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _productService.GetAsync(id);
            if (!response.HasError)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Errors);
        }
        
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List(string name)
        {
            var response = await _productService.GetProductsByNameAsync(name);
            return Ok(response.Data);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(ProductAddRequest request)
        {
            var response = await _productService.CreateAsync(request);
            if (!response.HasError)
            {
                return Created($"api/products/{response.Data.Id}",response.Data);    
            }

            return BadRequest(response.Errors);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(string id, ProductUpdateRequest request)
        {
            var response = await _productService.UpdateAsync(id, request);
            if (!response.HasError)
            {
                return Ok(response.Data);    
            }

            return BadRequest(response.Errors);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _productService.RemoveAsync(id);
            if(!response.HasError)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Errors);
        }
    }
}