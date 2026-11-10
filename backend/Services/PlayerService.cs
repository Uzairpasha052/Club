using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs.Player;
using backend.Interfaces;
using backend.Models;

namespace backend.Services;

public class PlayerService : IPlayerService
{
    private readonly AppDbContext _context;

    public PlayerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PlayerResponseDto>> GetAllPlayersAsync()
    {
        return await _context.Players
            .OrderBy(p => p.stdName)
            .Select(p => new PlayerResponseDto
            {
                Id = p.Id,
                stdName = p.stdName,
                stdFName = p.stdFName,
                stdAge = p.stdAge,
                stdGender = p.stdGender,
                stdContact = p.stdContact,
                stdBelt = p.stdBelt,
                stdHistory = p.stdHistory,
                stdReason = p.stdReason,
                stdFee = p.stdFee,
                stdStatus = p.stdStatus,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PlayerResponseDto?> GetPlayerByIdAsync(int id)
    {
        var player = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player == null)
            return null;

        return new PlayerResponseDto
        {
            Id = player.Id,
            stdName = player.stdName,
            stdFName = player.stdFName,
            stdAge = player.stdAge,
            stdGender = player.stdGender,
            stdContact = player.stdContact,
            stdBelt = player.stdBelt,
            stdHistory = player.stdHistory,
            stdReason = player.stdReason,
            stdFee = player.stdFee,
            stdStatus = player.stdStatus,
            CreatedAt = player.CreatedAt
        };
    }

    public async Task<PlayerResponseDto> CreatePlayerAsync(CreatePlayerDto dto)
    {
        var player = new Player
        {
            stdName = dto.stdName,
            stdFName = dto.stdFName,
            stdAge = dto.stdAge,
            stdGender = dto.stdGender,
            stdContact = dto.stdContact,
            stdBelt = dto.stdBelt,
            stdHistory = dto.stdHistory,
            stdReason = dto.stdReason,
            stdFee = dto.stdFee,
            stdStatus = "Active"
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return new PlayerResponseDto
        {
            Id = player.Id,
            stdName = player.stdName,
            stdFName = player.stdFName,
            stdAge = player.stdAge,
            stdGender = player.stdGender,
            stdContact = player.stdContact,
            stdBelt = player.stdBelt,
            stdHistory = player.stdHistory,
            stdReason = player.stdReason,
            stdFee = player.stdFee,
            stdStatus = player.stdStatus,
            CreatedAt = player.CreatedAt
        };
    }
 
    public async Task<PlayerResponseDto?> UpdatePlayerAsync(int id, UpdatePlayerDto dto)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
            return null;

        player.stdName = dto.stdName;
        player.stdFName = dto.stdFName;
        player.stdAge = dto.stdAge;
        player.stdGender = dto.stdGender;
        player.stdContact = dto.stdContact;
        player.stdBelt = dto.stdBelt;
        player.stdHistory = dto.stdHistory;
        player.stdReason = dto.stdReason;
        player.stdFee = dto.stdFee;
        player.stdStatus = dto.stdStatus;

        await _context.SaveChangesAsync();

        return new PlayerResponseDto
        {
            Id = player.Id,
            stdName = player.stdName,
            stdFName = player.stdFName,
            stdAge = player.stdAge,
            stdGender = player.stdGender,
            stdContact = player.stdContact,
            stdBelt = player.stdBelt,
            stdHistory = player.stdHistory,
            stdReason = player.stdReason,
            stdFee = player.stdFee,
            stdStatus = player.stdStatus,
            CreatedAt = player.CreatedAt
        };
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await _context.Players.FindAsync(id);

        if (player == null)
            return false;

        player.stdStatus = "InActive";

        // _context.Players.Remove(player);
        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        return true;

    }
}