namespace pp_back_codex.Models;

public class QrCode
{
    public int Id { get; set; }

    public int CartridgeId { get; set; }
    public Cartridge Cartridge { get; set; } = null!;

    public Guid Code { get; set; }
}
