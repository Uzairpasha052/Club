namespace backend.DTO.CashOut;

public class CashOutDto
{
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string Description { get; set; } = string.Empty;
}
