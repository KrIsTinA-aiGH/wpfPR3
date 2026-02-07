using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("waste_types")]
    public class WasteType
    {
        [Key]
        [Column("type_id")]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("type_name")]
        public string TypeName { get; set; } = null!;

        [Required]
        [Column("base_price_per_kg", TypeName = "decimal(10,2)")]
        public decimal BasePricePerKg { get; set; }
    }
}