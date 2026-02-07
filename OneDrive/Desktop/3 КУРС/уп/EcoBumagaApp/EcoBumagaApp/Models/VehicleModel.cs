using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("vehicle_models")]
    public class VehicleModel
    {
        [Key]
        [Column("model_id")]
        public int ModelId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("model_name")]
        public string ModelName { get; set; } = null!;

        [MaxLength(100)]
        [Column("manufacturer")]
        public string? Manufacturer { get; set; }
    }
}