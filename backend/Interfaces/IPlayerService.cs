using backend.DTOs.Player;

namespace backend.Interfaces;

public interface IPlayerService
{
    Task<List<PlayerResponseDto>> GetAllPlayersAsync();

    Task<PlayerResponseDto?> GetPlayerByIdAsync(int id);

    Task<PlayerResponseDto> CreatePlayerAsync(CreatePlayerDto dto);

    Task<PlayerResponseDto?> UpdatePlayerAsync(int id, UpdatePlayerDto dto);

    Task<bool> DeletePlayerAsync(int id);
}