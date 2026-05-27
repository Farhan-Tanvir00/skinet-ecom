using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await productRepository.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productRepository.AddProduct(product);
            if (await productRepository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
            }
            return BadRequest("Creation Failed");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !productRepository.ProductExists(id)) return BadRequest();
            productRepository.UpdateProduct(product);

            if(await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Update Failed");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            productRepository.DeleteProduct(product);

            if(await productRepository.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Deletion Failed");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            return Ok(await productRepository.GetProductBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {
            return Ok(await productRepository.GetProductTypesAsync());

        }
     }
}
