using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("measurement_units")]
    public class MeasurementUnit
    {
        [Key]
        [Column("unit_id")]
        public int UnitId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("unit_name")]
        public string UnitName { get; set; } = null!;
    }
}