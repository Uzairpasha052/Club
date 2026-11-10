namespace backend.DTO.Reports;

public class CashFlowReportDto
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public List<CashFlowCashInDto> CashIns { get; set; } = new();

    public List<CashFlowCashOutDto> CashOuts { get; set; } = new();

    public decimal TotalCashIn { get; set; }

    public decimal TotalCashOut { get; set; }

    public decimal ProfitAmount { get; set; }
}
