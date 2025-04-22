using Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }      
        public DbSet<ApplicationLogByMethodAndStatus> ApplicationLogByMethodAndStatuses { get; set; }
        public DbSet<ApplicationLogByPathFilter> ApplicationLogByPathFilters { get; set; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddAuditLogs();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<ApplicationLog>().ToTable("ApplicationLogs");
            modelBuilder.Entity<User>(userBuilder =>
            {
                userBuilder.ToTable("Users").HasKey(s => s.Id);
                userBuilder.Property(s => s.Id).HasColumnName("UserID");
                userBuilder.Property(s => s.Email).HasColumnName("Email").HasMaxLength(50).IsRequired();
                userBuilder.Property(s => s.Password).HasColumnName("Password").HasMaxLength(50).IsRequired();
                userBuilder.Property(s => s.RoleId).HasColumnName("RoleID");
                userBuilder.HasOne(s => s.Roles)
                            .WithMany(x => x.Users)
                            .HasForeignKey("RoleId");
            });
            modelBuilder.Entity<Role>(roleBuilder =>
            {
                roleBuilder.ToTable("Roles").HasKey(s => s.Id);
                roleBuilder.Property(s => s.Id).HasColumnName("RoleID");
                roleBuilder.Property(s => s.Name).HasColumnName("Name").HasMaxLength(50).IsRequired();
            });
            modelBuilder.Entity<ApplicationLogByMethodAndStatus>()
                        .HasNoKey()
                        .ToView(null);
            modelBuilder.Entity<ApplicationLogByPathFilter>()
                        .HasNoKey()
                        .ToView(null);    
        }

        private void AddAuditLogs()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog && e.Entity is not ApplicationLog &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                .ToList();

            foreach (var e in entries)
            {
                var audit = new AuditLog
                {
                    Timestamp = DateTime.UtcNow,
                    TableName = e.Entity.GetType().Name,
                    ActionType = e.State.ToString(),
                    KeyValues = JsonSerializer.Serialize(e.Properties
                            .Where(p => p.Metadata.IsPrimaryKey())
                            .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue)),

                    OldValues = e.State == EntityState.Modified || e.State == EntityState.Deleted
                        ? JsonSerializer.Serialize(e.GetDatabaseValues()!
                            .ToObject()
                            .GetType()
                            .GetProperties()
                            .ToDictionary(
                                prop => prop.Name,
                                prop => prop.GetValue(e.GetDatabaseValues()!.ToObject())!))
                        : null,

                    NewValues = e.State == EntityState.Added || e.State == EntityState.Modified
                        ? JsonSerializer.Serialize(e.CurrentValues.ToObject())
                        : null
                };

                AuditLogs.Add(audit);
            }
        }
    }
}
