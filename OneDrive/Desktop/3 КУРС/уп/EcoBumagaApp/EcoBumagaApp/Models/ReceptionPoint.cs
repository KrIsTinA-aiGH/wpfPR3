using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("reception_points")]
    public class ReceptionPoint
    {
        [Key]
        [Column("point_id")]
        public int PointId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("address")]
        public string Address { get; set; } = null!;

        [MaxLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }
    }
}