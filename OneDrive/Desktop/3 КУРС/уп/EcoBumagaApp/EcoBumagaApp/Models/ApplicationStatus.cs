using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("application_statuses")]
    public class ApplicationStatus
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