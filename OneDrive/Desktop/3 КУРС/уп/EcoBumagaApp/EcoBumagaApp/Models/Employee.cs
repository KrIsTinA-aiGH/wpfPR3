using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employee_id")]
        public int EmployeeId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("position_id")]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("employee_number")]
        public string EmployeeNumber { get; set; } = null!;

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        // Навигационные свойства
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("PositionId")]
        public virtual EmployeePosition? Position { get; set; }
    }
}