namespace backend.DTO.Reports;

public class CashOutReportDto
{
    public decimal Amount { get; set; }

    public DateOnly ExpenseDate { get; set; }

    public string Description { get; set; } = string.Empty;
}
