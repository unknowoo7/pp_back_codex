using System.ComponentModel.DataAnnotations;

namespace pp_back_codex.Contracts.Companies;

public sealed class CreateCompanyRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{10}(\d{2})?$")]
    public string Inn { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [RegularExpression(@"^\+?[0-9]{10,15}$")]
    [MaxLength(20)]
    public string Phone { get; init; } = string.Empty;
}
