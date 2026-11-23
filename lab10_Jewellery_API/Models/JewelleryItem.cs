using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab10_Jewellery_API.Models
{
    // This class becomes the "jewellery_items" table in the database
    [Table("jewellery_items")]
    public class JewelleryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Ring, Necklace, Earrings, Bracelet, Bangles ...
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        // Gold, Silver, Platinum, Diamond ...
        [Required]
        [StringLength(50)]
        public string Material { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("image")]
        public string Image { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Available";
    }
}
