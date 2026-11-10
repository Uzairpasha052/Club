namespace backend.Models;

public class CashIn
{
    
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    // Navigation Property
    public Player Player { get; set; } = null!;
}