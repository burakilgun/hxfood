using System.Collections.Generic;
using System.Threading.Tasks;
using HxFood.Api.Models.Entities;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HxFood.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var response = await _categoryService.GetAsync();
            return Ok(response.Data);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _categoryService.GetAsync(id);
            if (!response.HasError)
            {
                return Ok(response.Data);
            }

            return NotFound(response.Errors);
        }
        
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List(string name)
        {
            var response = await _categoryService.GetCategoriesByNameAsync(name);
            return Ok(response.Data);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CategoryAddRequest request)
        {
            var response = await _categoryService.CreateAsync(request);
            if (!response.HasError)
            {
                return Created($"api/categories/{response.Data.Id}",response.Data);    
            }

            return BadRequest(response.Errors);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(string id, CategoryUpdateRequest request)
        {
            var response = await _categoryService.UpdateAsync(id, request);
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
            var response = await _categoryService.RemoveAsync(id);
            if(!response.HasError)
            {
                return Ok(response.Data);
            }

            return BadRequest(response.Errors);
        }
    }
}