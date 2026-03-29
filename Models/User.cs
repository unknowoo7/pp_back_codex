using System.ComponentModel.DataAnnotations;

namespace pp_back_codex.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MiddleName { get; set; }

    [MaxLength(2048)]
    [Url]
    public string? AvatarUrl { get; set; }

    public ICollection<CartridgeHistory> CompletedWorks { get; set; } = new List<CartridgeHistory>();

    public string FullName => string.Join(" ", new[] { LastName, FirstName, MiddleName }
        .Where(value => !string.IsNullOrWhiteSpace(value)));
}
