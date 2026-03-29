using Microsoft.EntityFrameworkCore;
using pp_back_codex.Models;

namespace pp_back_codex.Data;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Company> Companies => Set<Company>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "Test Company 1", INN = "7700000001", Email = "company1@test.local", Phone = "+10000000001" },
            new Company { Id = 2, Name = "Test Company 2", INN = "7700000002", Email = "company2@test.local", Phone = "+10000000002" },
            new Company { Id = 3, Name = "Test Company 3", INN = "7700000003", Email = "company3@test.local", Phone = "+10000000003" },
            new Company { Id = 4, Name = "Test Company 4", INN = "7700000004", Email = "company4@test.local", Phone = "+10000000004" },
            new Company { Id = 5, Name = "Test Company 5", INN = "7700000005", Email = "company5@test.local", Phone = "+10000000005" },
            new Company { Id = 6, Name = "Test Company 6", INN = "7700000006", Email = "company6@test.local", Phone = "+10000000006" },
            new Company { Id = 7, Name = "Test Company 7", INN = "7700000007", Email = "company7@test.local", Phone = "+10000000007" },
            new Company { Id = 8, Name = "Test Company 8", INN = "7700000008", Email = "company8@test.local", Phone = "+10000000008" },
            new Company { Id = 9, Name = "Test Company 9", INN = "7700000009", Email = "company9@test.local", Phone = "+10000000009" },
            new Company { Id = 10, Name = "Test Company 10", INN = "7700000010", Email = "company10@test.local", Phone = "+10000000010" }
        );
    }
}
