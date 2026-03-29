using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp_back_codex.Models;

public class Cartridge : IValidatableObject
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public int? WorkGroupId { get; set; }
    public WorkGroup? WorkGroup { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string PrinterModel { get; set; } = string.Empty;

    public DateOnly AcceptedAt { get; set; }

    public DateOnly? DueDate { get; set; }

    public CartridgeStatus Status { get; set; } = CartridgeStatus.Idle;

    public QrCode QrCode { get; set; } = null!;

    public ICollection<CartridgeHistory> HistoryEntries { get; set; } = new List<CartridgeHistory>();

    [NotMapped]
    public int TotalCompletedWorks => HistoryEntries.Count;

    [NotMapped]
    public CartridgeHistory? LastCompletedWork => HistoryEntries
        .OrderByDescending(entry => entry.CompletedAt)
        .FirstOrDefault();

    [NotMapped]
    public string? LastCompletedWorkSummary => LastCompletedWork?.WorkSummary;

    [NotMapped]
    public string CurrentStatusDisplay => Status switch
    {
        CartridgeStatus.InProgress when DueDate.HasValue => $"В работе до {DueDate:dd.MM.yyyy}",
        CartridgeStatus.InProgress => "В работе",
        CartridgeStatus.Repaired => "Отремонтирован",
        _ => "Не в работе"
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DueDate.HasValue && DueDate.Value < AcceptedAt)
        {
            yield return new ValidationResult(
                "Крайний срок сдачи не может быть раньше даты приёма картриджа.",
                new[] { nameof(DueDate), nameof(AcceptedAt) });
        }

        if (Status == CartridgeStatus.InProgress)
        {
            if (!WorkGroupId.HasValue)
            {
                yield return new ValidationResult(
                    "Картридж в работе должен быть привязан к рабочей группе.",
                    new[] { nameof(WorkGroupId) });
            }

            if (!DueDate.HasValue)
            {
                yield return new ValidationResult(
                    "Для картриджа в работе нужно указать крайний срок сдачи.",
                    new[] { nameof(DueDate) });
            }
        }

        if (Status == CartridgeStatus.Idle && WorkGroupId.HasValue)
        {
            yield return new ValidationResult(
                "Картридж со статусом 'Не в работе' не должен быть привязан к рабочей группе.",
                new[] { nameof(Status), nameof(WorkGroupId) });
        }
    }
}
