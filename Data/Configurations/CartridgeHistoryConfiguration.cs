using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pp_back_codex.Models;

namespace pp_back_codex.Data.Configurations;

public class CartridgeHistoryConfiguration : IEntityTypeConfiguration<CartridgeHistory>
{
    public void Configure(EntityTypeBuilder<CartridgeHistory> builder)
    {
        builder.Property(history => history.Note).HasMaxLength(2000);

        builder.HasOne(history => history.Cartridge)
            .WithMany(cartridge => cartridge.HistoryEntries)
            .HasForeignKey(history => history.CartridgeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(history => history.Performer)
            .WithMany(user => user.CompletedWorks)
            .HasForeignKey(history => history.PerformerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
