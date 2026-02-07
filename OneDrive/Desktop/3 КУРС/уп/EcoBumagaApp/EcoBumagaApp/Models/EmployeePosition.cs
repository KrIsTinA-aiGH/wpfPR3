using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("employee_positions")]
    public class EmployeePosition
    {
        [Key]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("position_name")]
        public string PositionName { get; set; } = null!;
    }
}