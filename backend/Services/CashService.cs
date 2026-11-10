using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTO.CashIn;
using backend.DTO.CashOut;
using backend.Interfaces;
using backend.Models;

namespace backend.Services;

public class CashService: ICashService
{
    private readonly AppDbContext _context;

    public CashService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CashInAsync(CashInDto dto)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == dto.PlayerId);

        if (player == null)
        {
            return false;
        }

        if (dto.Amount <= 0)
        {
            return false;
        }

         var cashIn = new CashIn
        {
            PlayerId = dto.PlayerId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate == default
                ? DateTime.UtcNow
                : dto.PaymentDate
        };

        _context.CashIns.Add(cashIn);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CashOutAsync(
            CashOutDto dto)
    {
        if (dto.Amount <= 0)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            return false;
        }

        var cashOut = new CashOut
        {
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate == default
                ? DateTime.UtcNow
                : dto.PaymentDate,
            Description = dto.Description.Trim(),
        };

        _context.CashOuts.Add(cashOut);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<CashInListDto>> GetAllCashInAsync(int? year = null, int? month = null)
    {
        var targetYear = year ?? DateTime.Today.Year;
        var targetMonth = month ?? DateTime.Today.Month;

        var start = new DateTime(targetYear, targetMonth, 1);
        var end = start.AddMonths(1);

        return await _context.CashIns
            .Include(c => c.Player)
            .Where(c => c.PaymentDate >= start && c.PaymentDate < end)
            .OrderByDescending(c => c.PaymentDate)
            .Select(c => new CashInListDto
            {
                Id = c.Id,
                PlayerId = c.PlayerId,
                PlayerName = c.Player.stdName,
                Amount = c.Amount,
                PaymentDate = c.PaymentDate
            })
            .ToListAsync();
    }

    public async Task<List<CashOutDto>> GetAllCashOutAsync(int? year = null, int? month = null)
    {
        var targetYear = year ?? DateTime.Today.Year;
        var targetMonth = month ?? DateTime.Today.Month;

        var start = new DateTime(targetYear, targetMonth, 1);
        var end = start.AddMonths(1);

        return await _context.CashOuts
            .Where(c => c.PaymentDate >= start && c.PaymentDate < end)
            .OrderByDescending(c => c.PaymentDate)
            .Select(c => new CashOutDto
            {
                Amount = c.Amount,
                Description = c.Description,
                PaymentDate = c.PaymentDate,
            })
            .ToListAsync();
    }
}