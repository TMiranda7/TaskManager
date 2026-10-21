namespace RachaStats.Domain.Entities;

public enum PaymentStatus
{
    Pending = 1,
    Confirmed = 2,
    Rejected = 3
}

public class MatchPayment
{
    public Guid Id { get; set; }

    public Guid MatchId { get; set; }
    public Match Match { get; set; } = null!;

    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = null!;

    public string SourceMessage { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
