using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("vehicles")]
    public class Vehicle
    {
        [Key]
        [Column("vehicle_id")]
        public int VehicleId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("plate_number")]
        public string PlateNumber { get; set; } = null!;

        [Required]
        [Column("model_id")]
        public int ModelId { get; set; }

        [Required]
        [Column("capacity_kg", TypeName = "decimal(10,2)")]
        public decimal CapacityKg { get; set; }

        // Навигационные свойства
        [ForeignKey("ModelId")]
        public virtual VehicleModel? Model { get; set; }
    }
}