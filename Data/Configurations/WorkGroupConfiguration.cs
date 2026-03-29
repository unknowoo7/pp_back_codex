using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class WorkGroupConfiguration : IEntityTypeConfiguration<WorkGroup>
{
    public void Configure(EntityTypeBuilder<WorkGroup> builder)
    {
        builder.Property(group => group.Name).HasMaxLength(250).IsRequired();
        builder.HasIndex(group => group.CompanyId).IsUnique();
    }
}
