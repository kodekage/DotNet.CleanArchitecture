using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HR.LeaveManagement.Persistence.DatabaseContext;

public class HrDbContext : DbContext
{
    private readonly string _connectionString;
    private readonly bool _userConsoleLogger;

    public HrDbContext(string connectionString, bool userConsoleLogger)
    {
        _connectionString = connectionString;
        _userConsoleLogger = userConsoleLogger;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        optionsBuilder.UseLoggerFactory(_userConsoleLogger ? CreateLoggerFactory() : CreateEmptyLoggerFactory()).EnableSensitiveDataLogging();
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder
            .AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole());
    }

    private static ILoggerFactory CreateEmptyLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddFilter((_, _) => false));
    }

    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Automatically load all the entity configurations in persistence assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HrDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in base.ChangeTracker.Entries<BaseEntity>()
                     .Where(q => q.State is EntityState.Added or EntityState.Modified))
        {
            entity.Entity.DateModified = DateTime.Now;

            if (entity.State == EntityState.Added)
            {
                entity.Entity.DateCreated = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}