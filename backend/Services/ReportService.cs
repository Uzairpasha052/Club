using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs.Reports;
using backend.Services.Interfaces;
using backend.DTO.Reports;

namespace backend.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerRecordReportDto>> GetPlayerReportAsync(string status)
    {
        var query = _context.Players
            .AsNoTracking()
            .AsQueryable();

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (status.Equals("active", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => p.stdStatus.ToLower() == "active");
            }
            else if (status.Equals("inactive", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => p.stdStatus.ToLower() == "inactive");
            }
        }

        return await query
            .Select(p => new PlayerRecordReportDto
            {
                Id = p.Id,
                stdName = p.stdName,
                stdFName = p.stdFName,
                stdAge = p.stdAge,
                stdGender = p.stdGender,
                stdContact = p.stdContact,
                stdBelt = p.stdBelt,
                stdHistory = p.stdHistory,
                stdFee = p.stdFee,
                stdStatus = p.stdStatus,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<PlayerReasonReportDto>> GetPlayerReasonReportAsync()
    {
        return await _context.Players
            .AsNoTracking()
            .Where(p => !string.IsNullOrWhiteSpace(p.stdReason))
            .Select(p => new PlayerReasonReportDto
            {
                Id = p.Id,
                stdName = p.stdName,
                stdFName = p.stdFName,
                stdAge = p.stdAge,
                stdGender = p.stdGender,
                stdReason = p.stdReason
            })
            .ToListAsync();
    }

    public async Task<List<AttendanceReportDto>> GetAttendanceReportAsync(DateOnly StartDate, DateOnly EndDate)
    {
        return await _context.Attendances
            .AsNoTracking()
            .Where(a =>
                a.AttendanceDate >= StartDate && a.AttendanceDate <= EndDate
            )
            .Select(a => new AttendanceReportDto
            {
                PlayerName = a.Player.stdName,
                FatherName = a.Player.stdFName,
                Belt = a.Player.stdBelt,
                AttendanceDate = a.AttendanceDate,
                Status = a.Status ?? string.Empty,
                Remarks = a.Remarks ?? string.Empty
            })
            .OrderByDescending(a => a.AttendanceDate)
            .ThenBy(a => a.PlayerName)
            .ToListAsync();
    }

    public async Task<List<CashInReportDto>> GetCashInReportAsync(DateOnly startDate, DateOnly endDate)
    {
        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _context.CashIns
            .AsNoTracking()
            .Where(c =>
                c.PaymentDate >= startDateTime &&
                c.PaymentDate <= endDateTime)
            .Select(c => new CashInReportDto
            {
                PlayerName = c.Player.stdName,
                FatherName = c.Player.stdFName,
                Amount = c.Amount,
                PaymentDate = DateOnly.FromDateTime(c.PaymentDate)
            })
            .OrderByDescending(c => c.PaymentDate)
            .ThenBy(c => c.PlayerName)
            .ToListAsync();
    }


    public async Task<List<CashOutReportDto>> GetCashOutReportAsync(DateOnly startDate, DateOnly endDate)
    {

        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        return await _context.CashOuts
            .AsNoTracking()
            .Where(c =>
                c.PaymentDate >= startDateTime &&
                c.PaymentDate <= endDateTime)
            .Select(c => new CashOutReportDto
            {
                Amount = c.Amount,
                ExpenseDate = DateOnly.FromDateTime(c.PaymentDate),
                Description = c.Description
            })
            .OrderByDescending(c => c.ExpenseDate)
            .ToListAsync();
    }

    public async Task<CashFlowReportDto> GetCashFlowReportAsync(DateOnly startDate, DateOnly endDate)
    {

        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        var cashIns = await _context.CashIns
            .AsNoTracking()
            .Where(c =>
                c.PaymentDate >= startDateTime &&
                c.PaymentDate <= endDateTime)
            .Select(c => new CashFlowCashInDto
            {
                PlayerName = c.Player.stdName,
                FatherName = c.Player.stdFName,
                Amount = c.Amount,
                PaymentDate = DateOnly.FromDateTime(c.PaymentDate)
            })
            .OrderByDescending(c => c.PaymentDate)
            .ToListAsync();


        var cashOuts = await _context.CashOuts
            .AsNoTracking()
            .Where(c =>
                c.PaymentDate >= startDateTime &&
                c.PaymentDate <= endDateTime)
            .Select(c => new CashFlowCashOutDto
            {
                Amount = c.Amount,
                Description = c.Description,
                ExpenseDate = DateOnly.FromDateTime(c.PaymentDate)
            })
            .OrderByDescending(c => c.ExpenseDate)
            .ToListAsync();

        var totalCashIn = cashIns.Sum(c => c.Amount);

        var totalCashOut = cashOuts.Sum(c => c.Amount);

        var profitAmount = totalCashIn - totalCashOut;

        return new CashFlowReportDto
        {
            StartDate = startDate,
            EndDate = endDate,

            CashIns = cashIns,
            CashOuts = cashOuts,

            TotalCashIn = totalCashIn,
            TotalCashOut = totalCashOut,

            ProfitAmount = profitAmount
        };
    }

}
