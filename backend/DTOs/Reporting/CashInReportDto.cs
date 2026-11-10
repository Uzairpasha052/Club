namespace backend.DTO.Reports;

public class CashInReportDto
{
    public string PlayerName { get; set; } = string.Empty;

    public string FatherName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }
}
