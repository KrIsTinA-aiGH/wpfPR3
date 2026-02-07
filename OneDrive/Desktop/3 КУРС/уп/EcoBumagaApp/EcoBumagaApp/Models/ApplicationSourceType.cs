using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("application_source_types")]
    public class ApplicationSourceType
    {
        [Key]
        [Column("source_type_id")]
        public int SourceTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("source_type_name")]
        public string SourceTypeName { get; set; } = null!;
    }
}