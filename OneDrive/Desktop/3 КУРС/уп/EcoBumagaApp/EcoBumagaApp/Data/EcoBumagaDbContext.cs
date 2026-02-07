using EcoBumagaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoBumagaApp.Data
{
    public class EcoBumagaDbContext : DbContext
    {
        //наборы сущностей (таблицы базы данных)
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePosition> EmployeePositions { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationSourceType> ApplicationSourceTypes { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<ReceptionPoint> ReceptionPoints { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripStatus> TripStatuses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<WasteType> WasteTypes { get; set; }
        public DbSet<QualityCategory> QualityCategories { get; set; }
        public DbSet<BatchCondition> BatchConditions { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        //настройка подключения к базе данных
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //подключаемся к локальной базе PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Database=ecobumaga_db;Username=kriss;Password=123");
        }

        //перевод: настройка модели базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //применяем уникальные индексы и связи
            ConfigureUniqueIndexes(modelBuilder);
            ConfigureRelationships(modelBuilder);
        }

        //перевод: настройка уникальных индексов
        private void ConfigureUniqueIndexes(ModelBuilder modelBuilder)
        {
            //уникальность логинов, названий ролей, номеров сотрудников и т.д.
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeNumber).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.UserId).IsUnique();
            modelBuilder.Entity<EmployeePosition>().HasIndex(ep => ep.PositionName).IsUnique();

            modelBuilder.Entity<ApplicationSourceType>().HasIndex(ast => ast.SourceTypeName).IsUnique();
            modelBuilder.Entity<ApplicationStatus>().HasIndex(@as => @as.StatusName).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(mu => mu.UnitName).IsUnique();
            modelBuilder.Entity<ReceptionPoint>().HasIndex(rp => rp.Address).IsUnique();

            modelBuilder.Entity<Vehicle>().HasIndex(v => v.PlateNumber).IsUnique();
            modelBuilder.Entity<VehicleModel>().HasIndex(vm => vm.ModelName).IsUnique();
            modelBuilder.Entity<TripStatus>().HasIndex(ts => ts.StatusName).IsUnique();

            modelBuilder.Entity<ClientProfile>().HasIndex(cp => cp.UserId).IsUnique();
            modelBuilder.Entity<WasteType>().HasIndex(wt => wt.TypeName).IsUnique();
            modelBuilder.Entity<QualityCategory>().HasIndex(qc => qc.CategoryCode).IsUnique();
            modelBuilder.Entity<BatchCondition>().HasIndex(bc => bc.ConditionDescription).IsUnique();

            modelBuilder.Entity<InvoiceStatus>().HasIndex(@is => @is.StatusName).IsUnique();
            modelBuilder.Entity<PaymentMethod>().HasIndex(pm => pm.MethodName).IsUnique();
        }

        //перевод: настройка связей между сущностями
        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            //связь один-к-одному: профиль клиента → пользователь
            //при удалении пользователя удаляется и его профиль
            modelBuilder.Entity<ClientProfile>()
                .HasOne(cp => cp.User)
                .WithOne()
                .HasForeignKey<ClientProfile>(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}