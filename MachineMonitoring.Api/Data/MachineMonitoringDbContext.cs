using MachineMonitoring.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MachineMonitoring.Api.Data;

public class MachineMonitoringDbContext : IdentityDbContext<ApplicationUser>
{
    public MachineMonitoringDbContext(DbContextOptions<MachineMonitoringDbContext> options) : base(options)
    {

    }

    public DbSet<Machine> Machines { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<MachineLog> MachineLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasIndex(m => m.Name)
            .IsUnique()
            .HasDatabaseName("IX_Machine_Name_Unique");
        });
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasIndex(m => m.Name)
            .HasDatabaseName("IX_Operator_Name_Unique");
        });
        modelBuilder.Entity<MachineLog>(entity =>
        {
            entity.HasOne(mLog => mLog.Machine)
            .WithMany(m => m.MachineLogs)
            .HasForeignKey(mLog => mLog.MachineId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.Property(mLog => mLog.Temperature)
            .HasColumnType("decimal(5,2)");

            entity.HasIndex(mLog => mLog.MachineId)
            .HasDatabaseName("IX_MachineLogs_MachineId");

            entity.HasIndex(mLog => mLog.LogTime)
            .HasDatabaseName("IX_MachineLogs_LogTime");
        });
        seedData(modelBuilder);
    }

    private void seedData(ModelBuilder modelBuilder)
    {
        var baseDate = new DateTime(2025, 12, 26, 0, 0, 0, DateTimeKind.Utc);

        var machines = new List<Machine>
        {
            new Machine { Id = 1, Name = "CNC - 01" },
            new Machine { Id = 2, Name = "CNC - 02"},
            new Machine {Id = 3, Name = "MILLING - 01"},
            new Machine {Id = 4, Name = "MILLING - 02"},
            new Machine {Id = 5, Name = "PRESS - 01" },
            new Machine {Id = 6, Name = "PRESS - 02"},
            new Machine {Id = 7, Name = "ASSEMBLY - 01"}
        };

        modelBuilder.Entity<Machine>().HasData(machines);

        var operators = new List<Operator>
        {
            new Operator { Id = 1, Name = "Jhon Doe" },
            new Operator { Id = 2, Name = "Jane Doe"}
        };

        modelBuilder.Entity<Operator>().HasData(operators);

        var machineLogs = new List<MachineLog>
        {
            new MachineLog { Id = 1, MachineId = 1, OperatorId = 1, MachineStatus = Enums.MachineStatus.Running, ProducedPerMinute = 60, Temperature = 15.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 2, MachineId = 2, MachineStatus = Enums.MachineStatus.Idle, Temperature = 10.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 3, MachineId = 3, OperatorId = 2, MachineStatus = Enums.MachineStatus.Running, ProducedPerMinute = 50, Temperature = 15.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 4, MachineId = 4, MachineStatus = Enums.MachineStatus.Idle, Temperature = 10.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 5, MachineId = 5, MachineStatus = Enums.MachineStatus.Idle, Temperature = 10.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 6, MachineId = 6, MachineStatus = Enums.MachineStatus.Idle, Temperature = 10.0m, LogTime = baseDate.AddHours(10) },
            new MachineLog { Id = 7, MachineId = 7, MachineStatus = Enums.MachineStatus.Maintenance, Temperature = 5.0m, LogTime = baseDate.AddHours(10) }
        };
        modelBuilder.Entity<MachineLog>().HasData(machineLogs);

        // Auth User
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
        });

        var adminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
        var userRoleId = "2301D884-221A-4E7D-B509-0113DCC043E2";
        var managerRoleId = "2301D884-221A-4E7D-B509-0113DCC043E3";

        modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = managerRoleId
                }
            );

        var adminUserId = "2301D884-221A-4E7D-B509-0113DCC043A1";
        var hasher = new PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            UserName = "admin@electrindo.com",
            NormalizedUserName = "ADMIN@ELECTRINDO.COM",
            Email = "admin@electrindo.com",
            NormalizedEmail = "ADMIN@ELECTRINDO.COM",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = adminUserId
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );


    }
}