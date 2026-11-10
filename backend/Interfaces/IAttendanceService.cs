using backend.DTO.Attendance;
// using System.Security.Claims;

namespace backend.Interfaces;

public interface IAttendanceService
{

    // Get all players with today's attendance (for marking attendance)
    Task<List<TodayAttendanceDto>> GetTodayAttendanceAsync();

    // Save today's attendance
    Task<AttendanceResponseDto> SaveAttendanceAsync(List<MarkAttendanceDto> dto);

    // Complete attendance history for a calendar month
    Task<List<AdminAttendanceDto>> GetAttendanceHistoryAsync(int? year = null, int? month = null);

    // Individual player history
    Task<List<AttendanceHistoryDto>> GetPlayerAttendanceHistoryAsync(int playerId);

    // Admin update
    Task<AttendanceResponseDto> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceDto dto);

    // // Task<AttendanceResponseDto> CheckInAsync(ClaimsPrincipal user);
    // // Task<AttendanceResponseDto> CheckOutAsync(ClaimsPrincipal user);
    // Task<List<AttendanceHistoryDto>> GetMyAttendanceAsync(ClaimsPrincipal user);
    // // Task<List<AdminAttendanceDto>> GetAllAttendanceAsync();
    // Task<TodayAttendanceDto?> GetTodayAttendanceAsync(ClaimsPrincipal user);
    // Task<AttendanceResponseDto> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceDto dto);

}
