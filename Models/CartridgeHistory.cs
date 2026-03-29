using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pp_back_codex.Models;

public class CartridgeHistory : IValidatableObject
{
    public int Id { get; set; }

    public int CartridgeId { get; set; }
    public Cartridge Cartridge { get; set; } = null!;

    public int PerformerId { get; set; }
    public User Performer { get; set; } = null!;

    public DateTime CompletedAt { get; set; }

    public bool Refill { get; set; }

    public bool ReplaceChip { get; set; }

    public bool ReplacePhotoDrum { get; set; }

    public bool ReplaceWiperBlade { get; set; }

    public bool ReplaceDoctorBlade { get; set; }

    public bool ReplaceChargeRoller { get; set; }

    [MaxLength(2000)]
    public string? Note { get; set; }

    [NotMapped]
    public string CurrentCartridgeStatus => Cartridge?.CurrentStatusDisplay ?? "Не в работе";

    [NotMapped]
    public string WorkSummary
    {
        get
        {
            var parts = new List<string>();

            if (Refill) parts.Add("Заправка");
            if (ReplaceChip) parts.Add("Замена чипа");
            if (ReplacePhotoDrum) parts.Add("Замена фотобарабана");
            if (ReplaceWiperBlade) parts.Add("Замена ракеля");
            if (ReplaceDoctorBlade) parts.Add("Замена дозирующего лезвия");
            if (ReplaceChargeRoller) parts.Add("Замена вала заряда");
            if (!string.IsNullOrWhiteSpace(Note)) parts.Add(Note.Trim());

            return string.Join(", ", parts);
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var hasWork =
            Refill ||
            ReplaceChip ||
            ReplacePhotoDrum ||
            ReplaceWiperBlade ||
            ReplaceDoctorBlade ||
            ReplaceChargeRoller;

        if (!hasWork && string.IsNullOrWhiteSpace(Note))
        {
            yield return new ValidationResult(
                "История должна содержать хотя бы одну выполненную работу или примечание.",
                new[] { nameof(Note) });
        }
    }
}
