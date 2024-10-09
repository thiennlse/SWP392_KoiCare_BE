using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var product = await _productService.GetAllProduct();
            if (product == null)
            {
                return NotFound("empty Product");
            }
            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("phease input id >0");
            }
            var _product = await _productService.GetProductById(id);
            if (_product == null)
            {
                return NotFound("Product Does not exit");
            }
            return Ok(_product);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductRequestModel _product)
        {
            if (_product == null)
            {
                return BadRequest("please input product information");
            }
            Product product = new Product();
            
            product.UserId = _product.UserId;
            product.Name = _product.Name;
            product.Image = _product.Image;
            product.Cost = _product.Cost;
            product.Description = _product.Description;
            product.Origin = _product.Origin;
            product.Productivity = _product.Productivity;
            product.Code = _product.Code;
            product.InStock = _product.InStock;
            await _productService.AddNewProduct(product);
            return Created("Created", product);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var _product = await _productService.GetProductById(id);
            if (_product == null)
            {
                return NotFound("product is not exits");
            }
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody] ProductRequestModel _product, int id)
        {
            var product = await _productService.GetProductById(id);
            if (_product == null)
            {
                return NotFound("product is not exits");
            }
            product.Id = id;
            product.UserId = _product.UserId;
            product.Name = _product.Name;
            product.Image = _product.Image;
            product.Cost = _product.Cost;
            product.Description = _product.Description;
            product.Origin = _product.Origin;
            product.Productivity = _product.Productivity;
            product.Code = _product.Code;
            product.InStock = _product.InStock;

            await _productService.UpdateProduct(product);
            return Ok(product);
        }
    }
}
