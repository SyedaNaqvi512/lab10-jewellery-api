using lab10_Jewellery_API.Data;
using lab10_Jewellery_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab10_Jewellery_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelleryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public JewelleryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/jewellery
        // GET: api/jewellery?search=ring   (optional: search by name or category)
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] string? search)
        {
            var query = _context.JewelleryItems.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(j =>
                    j.Name.Contains(search) || j.Category.Contains(search));
            }

            var items = await query.OrderBy(j => j.Name).ToListAsync();
            return Ok(items);
        }

        // GET: api/jewellery/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.JewelleryItems.FindAsync(id);

            if (item == null)
                return NotFound("Jewellery item not found");

            return Ok(item);
        }

        // POST: api/jewellery   (form-data)
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromForm] JewelleryFormDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (input.Photo == null || input.Photo.Length == 0)
                return BadRequest("Jewellery image is required");

            string? imageError = ValidateImage(input.Photo);

            if (imageError != null)
                return BadRequest(imageError);

            var item = new JewelleryItem
            {
                Name = input.Name,
                Category = input.Category,
                Material = input.Material,
                Description = input.Description,
                Price = input.Price,
                Stock = input.Stock,
                Status = input.Status,
                Image = await SaveImage(input.Photo)
            };

            _context.JewelleryItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // PUT: api/jewellery/5   (form-data)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] JewelleryFormDto input)
        {
            var item = await _context.JewelleryItems.FindAsync(id);

            if (item == null)
                return NotFound("Jewellery item not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Replace the image only if a new one was sent
            if (input.Photo != null)
            {
                string? imageError = ValidateImage(input.Photo);

                if (imageError != null)
                    return BadRequest(imageError);

                string oldImage = item.Image;
                item.Image = await SaveImage(input.Photo);
                DeleteImage(oldImage);
            }

            item.Name = input.Name;
            item.Category = input.Category;
            item.Material = input.Material;
            item.Description = input.Description;
            item.Price = input.Price;
            item.Stock = input.Stock;
            item.Status = input.Status;

            await _context.SaveChangesAsync();

            return Ok(item);
        }

        // DELETE: api/jewellery/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.JewelleryItems.FindAsync(id);

            if (item == null)
                return NotFound("Jewellery item not found");

            DeleteImage(item.Image);

            _context.JewelleryItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Jewellery item deleted successfully");
        }

        // ---------- Helper methods ----------

        private string UploadsFolder() =>
            Path.Combine(
                _environment.WebRootPath ??
                Path.Combine(_environment.ContentRootPath, "wwwroot"),
                "uploads");

        // Checks file type and size
        private string? ValidateImage(IFormFile photo)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            string extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return "Only JPG, JPEG, PNG and WEBP images are allowed.";

            if (photo.Length == 0)
                return "Empty image is not allowed.";

            if (photo.Length > 5 * 1024 * 1024)
                return "Maximum image size is 5 MB.";

            return null;
        }

        // Saves the image with a random name and returns its URL path
        private async Task<string> SaveImage(IFormFile photo)
        {
            string folder = UploadsFolder();
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString()
                + Path.GetExtension(photo.FileName).ToLowerInvariant();

            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return "/uploads/" + fileName;
        }

        // Removes an image file from disk (if it exists)
        private void DeleteImage(string image)
        {
            if (string.IsNullOrWhiteSpace(image))
                return;

            string filePath = Path.Combine(UploadsFolder(), Path.GetFileName(image));

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
