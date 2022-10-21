using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        //GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //GET:/api/products?categoryId=1&minPrice=20&maxPrice=50
        [HttpGet]
        public async Task<ActionResult<ProductModel>> GetByFilter([FromQuery] FilterSearchModel filter)
        {
            var product = await _productService.GetByFilterAsync(filter);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel value)
        {
            try
            {
                await _productService.AddAsync(value);

                return CreatedAtAction(nameof(AddProduct), new { id = value.Id }, value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductModel value)
        {
            try
            {
                await _productService.UpdateAsync(value);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        //GET/api/products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            var categories = await _productService.GetAllProductCategoriesAsync();
            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        //POST/api/products/categories
        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
        {
            try
            {
                await _productService.AddCategoryAsync(value);

                return CreatedAtAction(nameof(AddCategory), new { id = value.Id }, value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT/api/products/categories{id}
        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdatCategory(int id, [FromBody] ProductCategoryModel value)
        {
            try
            {
                await _productService.UpdateCategoryAsync(value);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE/api/products/categories/{id} 
        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCateggory(int id)
        {
            await _productService.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
