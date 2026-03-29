using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.LastName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(user => user.MiddleName).HasMaxLength(100);
        builder.Property(user => user.AvatarUrl).HasMaxLength(2048);
    }
}
