using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp_back_codex.Models;

public class WorkGroup
{
    private const string BaseName = "Основная рабочая группа";

    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    [Required]
    [MaxLength(250)]
    public string Name { get; set; } = BaseName;

    public ICollection<Cartridge> ActiveCartridges { get; set; } = new List<Cartridge>();

    [NotMapped]
    public string DisplayName => string.IsNullOrWhiteSpace(Company?.Name)
        ? BaseName
        : $"{BaseName} {Company.Name}";

    [NotMapped]
    public IEnumerable<Cartridge> ActiveWorkCartridges => ActiveCartridges
        .Where(cartridge => cartridge.Status is CartridgeStatus.InProgress or CartridgeStatus.Repaired)
        .OrderBy(cartridge => cartridge.DueDate)
        .ThenBy(cartridge => cartridge.Name);

    [NotMapped]
    public int ActiveWorkCartridgesCount => ActiveWorkCartridges.Count();
}
