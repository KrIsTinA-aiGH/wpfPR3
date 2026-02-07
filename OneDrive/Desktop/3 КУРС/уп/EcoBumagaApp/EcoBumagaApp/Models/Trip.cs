using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace EcoBumagaApp.Models
{
    [Table("trips")]
    public class Trip
    {
        [Key]
        [Column("trip_id")]
        public int TripId { get; set; }

        [Required]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Required]
        [Column("driver_user_id")]
        public int DriverUserId { get; set; }

        [Required]
        [Column("vehicle_id")]
        public int VehicleId { get; set; }

        [Column("driver_employee_id")]
        public int? DriverEmployeeId { get; set; }

        [Column("dispatcher_employee_id")]
        public int? DispatcherEmployeeId { get; set; }

        [Required]
        [Column("trip_date")]
        public DateTime TripDate { get; set; }

        [Column("travel_time")]
        public TimeSpan? TravelTime { get; set; }

        [Column("planned_order")]
        public int? PlannedOrder { get; set; }

        [Required]
        [Column("trip_status_id")]
        public int TripStatusId { get; set; }

        // Навигационные свойства
        [ForeignKey("ApplicationId")]
        public virtual Application? Application { get; set; }

        [ForeignKey("DriverUserId")]
        public virtual User? DriverUser { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }

        [ForeignKey("TripStatusId")]
        public virtual TripStatus? TripStatus { get; set; }
    }
}