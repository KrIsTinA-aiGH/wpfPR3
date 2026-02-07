using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("invoices")]
    public class Invoice
    {
        [Key]
        [Column("invoice_id")]
        public int InvoiceId { get; set; }

        [Required]
        [Column("client_user_id")]
        public int ClientUserId { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Required]
        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Column("invoice_status_id")]
        public int InvoiceStatusId { get; set; }

        [Column("payment_time")]
        public TimeSpan? PaymentTime { get; set; }

        // Навигационные свойства
        [ForeignKey("ClientUserId")]
        public virtual User? ClientUser { get; set; }

        [ForeignKey("InvoiceStatusId")]
        public virtual InvoiceStatus? InvoiceStatus { get; set; }
    }
}