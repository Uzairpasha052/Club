namespace backend.DTO.Reports;

public class CashFlowCashOutDto
{
    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateOnly ExpenseDate { get; set; }
}
