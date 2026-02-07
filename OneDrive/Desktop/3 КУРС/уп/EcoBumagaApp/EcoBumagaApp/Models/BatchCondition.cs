using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("batch_conditions")]
    public class BatchCondition
    {
        [Key]
        [Column("condition_id")]
        public int ConditionId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("condition_description")]
        public string ConditionDescription { get; set; } = null!;
    }
}