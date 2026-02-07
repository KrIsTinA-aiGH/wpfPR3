using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EcoBumagaApp.Models
{
    [Table("applications")]
    public class Application
    {
        [Key]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("created_by_employee_id")]
        public int? CreatedByEmployeeId { get; set; }

        [Required]
        [Column("source_type_id")]
        public int SourceTypeId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("source_address")]
        public string SourceAddress { get; set; } = null!;

        [Column("reception_point_id")]
        public int? ReceptionPointId { get; set; }

        [Column("estimated_weight", TypeName = "decimal(10,2)")]
        public decimal? EstimatedWeight { get; set; }

        [Column("unit_id")]
        public int? UnitId { get; set; }

        [Required]
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [Column("bonuses_awarded", TypeName = "decimal(10,2)")]
        public decimal BonusesAwarded { get; set; }

        // Навигационные свойства
        [ForeignKey("UserId")]
        public virtual User? ClientUser { get; set; }

        [ForeignKey("CreatedByEmployeeId")]
        public virtual Employee? CreatedByEmployee { get; set; }

        [ForeignKey("SourceTypeId")]
        public virtual ApplicationSourceType? SourceType { get; set; }

        [ForeignKey("StatusId")]
        public virtual ApplicationStatus? Status { get; set; }

        [ForeignKey("ReceptionPointId")]
        public virtual ReceptionPoint? ReceptionPoint { get; set; }

        [ForeignKey("UnitId")]
        public virtual MeasurementUnit? Unit { get; set; }
    }
}