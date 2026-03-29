using System.ComponentModel.DataAnnotations;

namespace pp_back_codex.Models;

public class Company
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{10}(\d{2})?$")]
    public string Inn { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?[0-9]{10,15}$")]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public WorkGroup? WorkGroup { get; set; }

    public ICollection<Cartridge> Cartridges { get; set; } = new List<Cartridge>();
}
