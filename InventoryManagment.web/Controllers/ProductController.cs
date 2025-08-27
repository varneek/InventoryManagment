using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryManagment.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Product = InventoryManagment.web.Models.Product;
using InventoryManagment.web.Models;

namespace InventoryManagment.web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext db;

        public ProductController(AppDbContext context)
        {
            this.db = context;
        }
        [HttpGet]
        [Route("api/Product/GetAllProducts")]
        public async Task<IActionResult> GetAllProducts(string? category, string? sortOrder)
        {
            var query = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            if (!string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder.ToLower() == "asc")
                    query = query.OrderBy(p => p.Price);
                else if (sortOrder.ToLower() == "desc")
                    query = query.OrderByDescending(p => p.Price);
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        [Route("api/Product/CreateProduct")]
        public async Task<IActionResult> CreateProduct(Product obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid");
            }
            db.Products.Add(obj);
            await db.SaveChangesAsync();

            return Ok(obj);
        }

        [HttpPut]
        [Route("api/Product/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, Product obj)
        {
            if (id != obj.Id)
            {
                return BadRequest("Id is not valid");
            }
            db.Entry(obj).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(obj);
        }
        [HttpDelete]
        [Route("api/Product/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleProductIsActive([FromRoute] int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found" });
            }
            product.IsActive = !product.IsActive;
            await db.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Search term cannot be empty");
            }
            var products = await db.Products
                .Where(p => p.Name.Contains(name)) 
                .ToListAsync();

            if (products == null || !products.Any())
                return NotFound($"No products found matching '{name}'");

            return Ok(products);
        }

    }
}

