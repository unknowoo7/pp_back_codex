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

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(company => company.Name).HasMaxLength(200).IsRequired();
            entity.Property(company => company.Inn).HasMaxLength(12).IsRequired();
            entity.Property(company => company.Email).HasMaxLength(256).IsRequired();
            entity.Property(company => company.Phone).HasMaxLength(20).IsRequired();

            entity.HasIndex(company => company.Inn).IsUnique();
            entity.HasIndex(company => company.Email).IsUnique();

            entity.HasOne(company => company.WorkGroup)
                .WithOne(group => group.Company)
                .HasForeignKey<WorkGroup>(group => group.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkGroup>(entity =>
        {
            entity.Property(group => group.Name).HasMaxLength(250).IsRequired();
            entity.HasIndex(group => group.CompanyId).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(user => user.LastName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.MiddleName).HasMaxLength(100);
            entity.Property(user => user.AvatarUrl).HasMaxLength(2048);
        });

        modelBuilder.Entity<Cartridge>(entity =>
        {
            entity.Property(cartridge => cartridge.Name).HasMaxLength(200).IsRequired();
            entity.Property(cartridge => cartridge.PrinterModel).HasMaxLength(200).IsRequired();
            entity.Property(cartridge => cartridge.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

            entity.HasOne(cartridge => cartridge.Company)
                .WithMany(company => company.Cartridges)
                .HasForeignKey(cartridge => cartridge.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cartridge => cartridge.WorkGroup)
                .WithMany(group => group.ActiveCartridges)
                .HasForeignKey(cartridge => cartridge.WorkGroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<QrCode>(entity =>
        {
            entity.Property(code => code.Code).HasMaxLength(256).IsRequired();
            entity.HasIndex(code => code.Code).IsUnique();
            entity.HasIndex(code => code.CartridgeId).IsUnique();

            entity.HasOne(code => code.Cartridge)
                .WithOne(cartridge => cartridge.QrCode)
                .HasForeignKey<QrCode>(code => code.CartridgeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartridgeHistory>(entity =>
        {
            entity.Property(history => history.Note).HasMaxLength(2000);

            entity.HasOne(history => history.Cartridge)
                .WithMany(cartridge => cartridge.HistoryEntries)
                .HasForeignKey(history => history.CartridgeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(history => history.Performer)
                .WithMany(user => user.CompletedWorks)
                .HasForeignKey(history => history.PerformerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private void SyncWorkGroupNames()
    {
        var workGroups = ChangeTracker.Entries<WorkGroup>()
            .Where(entry => entry.State is EntityState.Added or EntityState.Modified)
            .Select(entry => entry.Entity);

        foreach (var workGroup in workGroups)
        {
            var companyName = workGroup.Company?.Name?.Trim();
            workGroup.Name = string.IsNullOrWhiteSpace(companyName)
                ? "Основная рабочая группа"
                : $"Основная рабочая группа {companyName}";
        }
    }
}
