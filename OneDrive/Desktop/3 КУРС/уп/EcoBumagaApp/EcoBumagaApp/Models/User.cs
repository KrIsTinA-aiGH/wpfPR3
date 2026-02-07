using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoBumagaApp.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(100)]
        [Column("last_name")]
        public string? LastName { get; set; }

        [MaxLength(100)]
        [Column("first_name")]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        [Column("patronymic")]
        public string? Patronymic { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("login")]
        public string Login { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}