using System.ComponentModel.DataAnnotations;

namespace lab10_Jewellery_API.Models
{
    // What the client sends (form-data) when adding or updating an item.
    // Kept separate from JewelleryItem so the image path is only set by the server.
    public class JewelleryFormDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Material { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000000)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Available";

        // Photo (required when adding, optional when updating)
        public IFormFile? Photo { get; set; }
    }
}
