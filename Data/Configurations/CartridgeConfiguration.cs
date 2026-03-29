using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class CartridgeConfiguration : IEntityTypeConfiguration<Cartridge>
{
    public void Configure(EntityTypeBuilder<Cartridge> builder)
    {
        builder.Property(cartridge => cartridge.Name).HasMaxLength(200).IsRequired();
        builder.Property(cartridge => cartridge.PrinterModel).HasMaxLength(200).IsRequired();
        builder.Property(cartridge => cartridge.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.HasOne(cartridge => cartridge.Company)
            .WithMany(company => company.Cartridges)
            .HasForeignKey(cartridge => cartridge.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cartridge => cartridge.WorkGroup)
            .WithMany(group => group.ActiveCartridges)
            .HasForeignKey(cartridge => cartridge.WorkGroupId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
