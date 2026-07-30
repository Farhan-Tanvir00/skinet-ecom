using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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
        private readonly IGenericRepository<Product> genericRepository;
        public ProductsController(IGenericRepository<Product> GenericRepository) 
        {
            genericRepository = GenericRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type);
            return Ok(await genericRepository.GetAllWithSpec(spec));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            genericRepository.Add(product);
            if (await genericRepository.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new {id = product.Id}, product);
            }
            return BadRequest("Creation Failed");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !genericRepository.Exists(id)) return BadRequest();
            genericRepository.Update(product);

            if(await genericRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Update Failed");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);
            if (product == null) return NotFound();

            genericRepository.Remove(product);

            if(await genericRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Deletion Failed");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            //TODO: Implement
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductTypes()
        {
            //TODO: Implement
            return Ok();

        }
     }
}
