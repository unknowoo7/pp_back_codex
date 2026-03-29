namespace pp_back_codex.Contracts.Companies;

public sealed class CompanyDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Inn { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public int? WorkGroupId { get; init; }
    public string? WorkGroupName { get; init; }
}
