using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("quality_categories")]
    public class QualityCategory
    {
        [Key]
        [Column("category_id")]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(1)]
        [Column("category_code")]
        public string CategoryCode { get; set; } = null!;
    }
}