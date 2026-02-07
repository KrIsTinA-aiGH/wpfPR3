using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("trip_statuses")]
    public class TripStatus
    {
        [Key]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("status_name")]
        public string StatusName { get; set; } = null!;
    }
}