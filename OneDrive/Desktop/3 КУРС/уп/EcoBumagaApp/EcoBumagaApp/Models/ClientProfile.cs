using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("client_profiles")]
    public class ClientProfile
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(255)]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [Column("bonus_balance", TypeName = "decimal(10,2)")]
        public decimal BonusBalance { get; set; } = 0.00m;

        [MaxLength(255)]
        [Column("organization_name")]
        public string? OrganizationName { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("client_type")]
        public string ClientType { get; set; } = "individual";

        // Навигационное свойство
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}