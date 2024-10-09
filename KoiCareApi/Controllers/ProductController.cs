using BusinessObject.Models;
using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Service.Interface;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemberService _memberService;
        public ProductController(IProductService productService, IMemberService memberService)
        {
            _productService = productService;
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct(int page = 1, int pagesize = 10, string? searchTerm = null)
        {
            var product = await _productService.GetAllProduct(page, pagesize, searchTerm);
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
            Product product = new Product
            {
                Image = _product.Image,
                UserId = _product.UserId,
                Name = _product.Name,
                Cost = _product.Cost,
                Description = _product.Description,
                Origin = _product.Origin,
                Productivity = _product.Productivity,
                Code = _product.Code,
                InStock = _product.InStock
            };

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

            product.UserId = _product.UserId;
            product.Name = _product.Name;
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
