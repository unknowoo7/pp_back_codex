using Microsoft.EntityFrameworkCore;
using pp_back_codex.Models;

namespace pp_back_codex.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Cartridge> Cartridges => Set<Cartridge>();
    public DbSet<CartridgeHistory> CartridgeHistories => Set<CartridgeHistory>();
    public DbSet<QrCode> QrCodes => Set<QrCode>();
    public DbSet<User> Users => Set<User>();
    public DbSet<WorkGroup> WorkGroups => Set<WorkGroup>();

    public override int SaveChanges()
    {
        SyncWorkGroupNames();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SyncWorkGroupNames();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SyncWorkGroupNames();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SyncWorkGroupNames();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    private void SyncWorkGroupNames()
    {
        var workGroups = ChangeTracker.Entries<WorkGroup>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .Select(entry => entry.Entity);

        var companyWorkGroups = ChangeTracker.Entries<Company>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .Select(entry => entry.Entity.WorkGroup)
            .Where(workGroup => workGroup is not null)
            .Cast<WorkGroup>();

        foreach (var workGroup in workGroups.Concat(companyWorkGroups).Distinct())
        {
            var companyName = workGroup.Company?.Name?.Trim();
            workGroup.Name = string.IsNullOrWhiteSpace(companyName)
                ? "Основная рабочая группа"
                : $"Основная рабочая группа {companyName}";
        }
    }
}
