using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Required]
        [Column("invoice_id")]
        public int InvoiceId { get; set; }

        [Required]
        [Column("payment_date")]
        public DateTime PaymentDate { get; set; }

        [Column("payment_time")]
        public TimeSpan? PaymentTime { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column("payment_method_id")]
        public int PaymentMethodId { get; set; }

        [Column("received_by_employee_id")]
        public int? ReceivedByEmployeeId { get; set; }

        // Навигационные свойства
        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }

        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod? PaymentMethod { get; set; }

        [ForeignKey("ReceivedByEmployeeId")]
        public virtual Employee? ReceivedByEmployee { get; set; }
    }
}