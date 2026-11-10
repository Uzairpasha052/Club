using backend.DTO.Reports;
using backend.DTOs.Reports;

namespace backend.Services.Interfaces;

public interface IReportService
{
    Task<List<PlayerRecordReportDto>> GetPlayerReportAsync(string status);

    Task<List<PlayerReasonReportDto>> GetPlayerReasonReportAsync();

    Task<List<AttendanceReportDto>> GetAttendanceReportAsync( DateOnly StartDate, DateOnly EndDate);

    // For Cash

    Task<List<CashInReportDto>> GetCashInReportAsync( DateOnly StartDate, DateOnly EndDate);

    Task<List<CashOutReportDto>> GetCashOutReportAsync( DateOnly startDate, DateOnly EndDate);

    Task<CashFlowReportDto> GetCashFlowReportAsync(DateOnly startDate, DateOnly endDate);

}
