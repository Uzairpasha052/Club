using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services.Interfaces;
using QuestPDF.Fluent;
using backend.Report;

namespace backend.Controllers;

[Route("api/reports")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("player-record")]
    public async Task<IActionResult> GetPlayerRecordReport(
        [FromQuery] string status = "all")
    {
        var players = await _reportService
            .GetPlayerReportAsync(status);

        var document = new PlayerRecordReportDocuments(players, status);
        var pdfBytes = document.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Player-Record-Report-{status}.pdf"
        );

        // return Ok(players);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("player-reason")]
    public async Task<IActionResult> GetPlayerReasonReport()
    {
        var players = await _reportService
            .GetPlayerReasonReportAsync();

        var document = new PlayerReasonReportDocument(players);
        var pdfBytes = document.GeneratePdf();
        return File(
            pdfBytes,
            "application/pdf",
            "Player-Reason-Report.pdf"
        );
        // return Ok(players);
    }

    [Authorize(Roles ="Admin")]
    [HttpGet("attendance-record")]
    public async Task<IActionResult> GetAttendanceReport(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {
        if(startDate > endDate)
        {
            return BadRequest("StartDate cannot be grater than EndDate");
        }

        var attendance = await _reportService
            .GetAttendanceReportAsync(startDate, endDate);

        var document = new AttendanceReportDocument
        (
            attendance,
            startDate,
            endDate
        );

        var pdfBytes = document.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Attendance-Report-{startDate:dd-MM-yyyy}-to-{endDate:dd-MM-yyyy}.pdf"
        );
    }

    [HttpGet("cash-in")]
    public async Task<IActionResult> GetCashInReport(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(
                "Start date cannot be greater than end date.");
        }

        var cashIns = await _reportService
            .GetCashInReportAsync(startDate, endDate);

        var document = new CashInReportDocument(
            cashIns,
            startDate,
            endDate
        );

        var pdfBytes = document.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Cash-In-Report-{startDate:dd-MM-yyyy}-to-{endDate:dd-MM-yyyy}.pdf"
        );
    }


    [HttpGet("cash-out")]
    public async Task<IActionResult> GetCashOutReport(
    [FromQuery] DateOnly startDate,
    [FromQuery] DateOnly endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(
                "Start date cannot be greater than end date.");
        }

        var cashOuts = await _reportService
            .GetCashOutReportAsync(startDate, endDate);

        var document = new CashOutReportDocument(
            cashOuts,
            startDate,
            endDate
        );

        var pdfBytes = document.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Cash-Out-Report-{startDate:dd-MM-yyyy}-to-{endDate:dd-MM-yyyy}.pdf"
        );
    }


    [HttpGet("cash-flow")]
    public async Task<IActionResult> GetCashFlowReport(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest(
                "Start date cannot be greater than end date.");
        }

        var report = await _reportService
            .GetCashFlowReportAsync(startDate, endDate);

        var document = new CashFlowReportDocument(report);

        var pdfBytes = document.GeneratePdf();

        return File(
            pdfBytes,
            "application/pdf",
            $"Cash-Flow-Report-{startDate:dd-MM-yyyy}-to-{endDate:dd-MM-yyyy}.pdf"
        );
    }

}
