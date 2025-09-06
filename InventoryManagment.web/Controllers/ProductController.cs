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
        public async Task<IActionResult> GetAllProducts(string? category)
        {
            var query = db.Products.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            var products = await query.ToListAsync();
            if (!products.Any())
            {
                return NotFound(new { Message = "No active products found." });
            }

            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetAllInactiveProducts")]
        public async Task<IActionResult> GetAllInactiveProducts(string? category)
        {
            var query = db.Products.Where(p => !p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            var products = await query.ToListAsync();
            if (!products.Any())
            {
                return NotFound(new { Message = "No Inactive products found." });
            }

            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await db.Products
                .Where(p => p.IsActive && p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { Message = "Product not found or inactive." });
            }
            return Ok(product);
        }

        [HttpPost]
        [Route("api/Product/CreateProduct")]
        public async Task<IActionResult> CreateProduct(Product obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Model state is not valid." });
            }
            obj.IsActive = true;
            db.Products.Add(obj);
            await db.SaveChangesAsync();

            return Ok(new { Message = $"Product created successfully", obj.Id});
        }

        [HttpPut]
        [Route("api/Product/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, Product obj)
        {
            if (id != obj.Id)
            {
                return BadRequest(new { Message = "Id in URL does not match the product Id." });
            }
            db.Entry(obj).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(new { Message = "Product updated successfully."});
        }

        [HttpDelete]
        [Route("api/Product/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null || product.IsActive)
            {
                return NotFound(new { Message = "Product not found or active." });
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Ok(new { Message = "Product deleted successfully."});
        }

        [HttpPatch("toggle/{id}")]
        public async Task<IActionResult> ToggleProductIsActive([FromRoute] int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "Product not found." });
            }
            product.IsActive = !product.IsActive;
            await db.SaveChangesAsync();

            return Ok(new { Message = $"Product status is {product.IsActive}"});
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? name, [FromQuery] string? category)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(category))
            {
                return BadRequest(new { Message = "You must provide either a name or a category to search." });
            }

            var query = db.Products.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category.Contains(category));
            }

            var products = await query.ToListAsync();

            if (!products.Any())
                return NotFound(new { Message = "No active products found matching the search criteria." });

            return Ok(products);
        }

    }
}
        
       
