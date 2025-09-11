using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryManagment.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Product = InventoryManagment.web.Models.Product;
using InventoryManagment.web.Models;
using InventoryManagment.web.Dtos;

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
        public async Task<IActionResult> GetAllProducts(string? category, string? sortBy, string? sortOrder = "asc")
        {
            var query = db.Products.Where(p => p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            query = ApplySorting(query, sortBy, sortOrder);

            var products = await query.ToListAsync();
            if (!products.Any())
            {
                return NotFound(new { Message = "No active products found." });
            }

            return Ok(products);
        }

        [HttpGet]
        [Route("api/Product/GetAllInactiveProducts")]
        public async Task<IActionResult> GetAllInactiveProducts(string? category, string? sortBy, string? sortOrder = "asc")
        {
            var query = db.Products.Where(p => !p.IsActive).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            query = ApplySorting(query, sortBy, sortOrder);

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
        public async Task<IActionResult> CreateProduct(ProductRequest req)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(new { Message = "Model state is not valid." });
            }

            if (!decimal.TryParse(req.Price, out var parsedPrice))
            {
                return BadRequest(new { Message = "Price must be a integer." });
            }

            if (!int.TryParse(req.StockQuantity, out var StockQuantity))
            {
                return BadRequest(new { Message = "Stock must be a Integer." });
            }

            var product = new Product
            {
                Name = req.Name,
                Price = parsedPrice,
                IsActive = true,
                StockQuantity = StockQuantity,
                Category = req.Category,
                Description = req.Description
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Ok(new { Message = "Product created successfully", product.Id });
        }

        [HttpPut]
        [Route("api/Product/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductResponse res)
        {
            if (!int.TryParse(res.Id, out var id))
            {
                return Ok(new { Message = "Invalid Id" });
            }

            if (!decimal.TryParse(res.Price, out var Price))
            {
                return BadRequest(new { Message = "Price must be a integer." });
            }

            if (!int.TryParse(res.StockQuantity, out var StockQuantity))
            {
                return BadRequest(new { Message = "Stock must be a Integer." });
            }

            var product = await db.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { Message = "Product not found." });


            product.Name = res.Name;
            product.Price = Price;
            product.StockQuantity = StockQuantity;
            product.Category = res.Category;
            product.Description = res.Description;


            await db.SaveChangesAsync();

            return Ok(new { Message = "Product updated successfully." });
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
            return Ok(new { Message = "Product deleted successfully." });
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

            return Ok(new { Message = $"Product status is {product.IsActive}" });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? name, [FromQuery] string? category,
            [FromQuery] string? sortBy, [FromQuery] string? sortOrder = "asc")
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

            query = ApplySorting(query, sortBy, sortOrder);

            var products = await query.ToListAsync();

            if (!products.Any())
                return NotFound(new { Message = "No active products found matching the search criteria." });

            return Ok(products);
        }
        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy, string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return query; 

            var isDescending = sortOrder?.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "id" => isDescending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                "name" => isDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => isDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "category" => isDescending ? query.OrderByDescending(p => p.Category) : query.OrderBy(p => p.Category),
                _ => query
            };
        }
    }
}