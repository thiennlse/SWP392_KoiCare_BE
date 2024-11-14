using BusinessObject.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Validation.Product;
using FluentValidation;
using FluentValidation.Results;
using BusinessObject.Models;
using BusinessObject.ResponseModel;

namespace KoiCareApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMemberService _memberService;
        private readonly ProductValidation _productValidation;
        public ProductController(IProductService productService, IMemberService memberService, ProductValidation productValidation)
        {
            _productService = productService;
            _memberService = memberService;
            _productValidation = productValidation;
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
            try
            {
                ValidationResult validationResult = _productValidation.Validate(_product);
                if (validationResult.IsValid)
                {
                    await _productService.AddNewProduct(_product);
                    return Ok("Created");
                }
                var error = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                });
                return BadRequest(error);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok("Deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateById([FromBody] ProductRequestModel _product, int id)
        {
            try
            {
                ValidationResult validationResult = _productValidation.Validate(_product);
                if (validationResult.IsValid)
                {
                    await _productService.UpdateProduct(id, _product);
                    return Ok("Updated Successful");
                }
                var error = validationResult.Errors.Select(e => (object)new
                {
                    e.PropertyName,
                    e.ErrorMessage
                });
                return BadRequest(error);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getlistproductformlistid")]
        public async Task<IActionResult> GetListProductbyListProductid( List<int> listProductId)
        {
            try
            {
                List<Product> ListProductResult = await _productService.GetListProductbyListProductid(listProductId);
               return Ok(ListProductResult);
                
            }
            catch (Exception ex) {
            return BadRequest(ex.Message);
            }

        }
    }
}
