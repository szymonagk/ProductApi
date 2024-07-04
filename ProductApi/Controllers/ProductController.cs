using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Entities;
using ProductApi.Interfaces;
using ProductApi.Models;
using System.Security.Claims;

namespace ProductApi.Controllers
{
    [Route("products")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductWithUserFlagDTO>> GetAllProductsWithUserFlag()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(_productService.GetAllProductsWithUserFlag(userId));
        }

        [HttpGet("id")]
        public ActionResult<Product> GetProductById(int id)
        {
            return Ok(_productService.GetById(id));
        }

        [HttpPost("changeFlag")]
        public ActionResult<string> UpdateFlag([FromBody] ProductIdAndUserFlagDTO productDTO)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(_productService.AddOrUpdateFlaggedCountAndReturnMessage(productDTO, userId));
        }
    }
}
