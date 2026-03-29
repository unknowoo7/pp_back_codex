using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(company => company.Name).HasMaxLength(200).IsRequired();
        builder.Property(company => company.Inn).HasMaxLength(12).IsRequired();
        builder.Property(company => company.Email).HasMaxLength(256).IsRequired();
        builder.Property(company => company.Phone).HasMaxLength(20).IsRequired();

        builder.HasIndex(company => company.Inn).IsUnique();
        builder.HasIndex(company => company.Email).IsUnique();

        builder.HasOne(company => company.WorkGroup)
            .WithOne(group => group.Company)
            .HasForeignKey<WorkGroup>(group => group.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
