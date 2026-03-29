using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class QrCodeConfiguration : IEntityTypeConfiguration<QrCode>
{
    public void Configure(EntityTypeBuilder<QrCode> builder)
    {
        builder.Property(code => code.Code).HasMaxLength(256).IsRequired();
        builder.HasIndex(code => code.Code).IsUnique();
        builder.HasIndex(code => code.CartridgeId).IsUnique();

        builder.HasOne(code => code.Cartridge)
            .WithOne(cartridge => cartridge.QrCode)
            .HasForeignKey<QrCode>(code => code.CartridgeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
