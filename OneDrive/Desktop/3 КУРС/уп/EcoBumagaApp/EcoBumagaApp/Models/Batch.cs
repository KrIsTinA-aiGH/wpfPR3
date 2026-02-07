using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("batches")]
    public class Batch
    {
        [Key]
        [Column("batch_id")]
        public int BatchId { get; set; }

        [Required]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Required]
        [Column("waste_type_id")]
        public int WasteTypeId { get; set; }

        [Required]
        [Column("unit_id")]
        public int UnitId { get; set; }

        [Required]
        [Column("quality_category_id")]
        public int QualityCategoryId { get; set; }

        [Column("condition_id")]
        public int? ConditionId { get; set; }

        [Required]
        [Column("final_price_per_kg", TypeName = "decimal(10,2)")]
        public decimal FinalPricePerKg { get; set; }

        [Column("accepted_by_employee_id")]
        public int? AcceptedByEmployeeId { get; set; }

        [Required]
        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column("accepted_date")]
        public DateTime AcceptedDate { get; set; }

        [Column("accepted_time")]
        public TimeSpan? AcceptedTime { get; set; }

        [Column("actual_weight", TypeName = "decimal(10,2)")]
        public decimal? ActualWeight { get; set; }

        [Column("reception_point_id")]
        public int? ReceptionPointId { get; set; }

        // Навигационные свойства
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }

        [ForeignKey("WasteTypeId")]
        public virtual WasteType? WasteType { get; set; }

        [ForeignKey("UnitId")]
        public virtual MeasurementUnit? Unit { get; set; }

        [ForeignKey("QualityCategoryId")]
        public virtual QualityCategory? QualityCategory { get; set; }

        [ForeignKey("ConditionId")]
        public virtual BatchCondition? Condition { get; set; }

        [ForeignKey("AcceptedByEmployeeId")]
        public virtual Employee? AcceptedByEmployee { get; set; }

        [ForeignKey("ReceptionPointId")]
        public virtual ReceptionPoint? ReceptionPoint { get; set; }
    }
}