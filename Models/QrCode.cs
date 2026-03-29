using System.ComponentModel.DataAnnotations;

namespace pp_back_codex.Models;

public class QrCode
{
    public int Id { get; set; }

    public int CartridgeId { get; set; }
    public Cartridge Cartridge { get; set; } = null!;

    [Required]
    [MaxLength(256)]
    [RegularExpression(@"^[A-Za-z0-9\-_:.]+$")]
    public string Code { get; set; } = string.Empty;
}
