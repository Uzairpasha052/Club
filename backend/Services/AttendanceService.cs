using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTO.Attendance;
using backend.Interfaces;
using backend.Models;

namespace backend.Services;

public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _context;

    public AttendanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodayAttendanceDto>> GetTodayAttendanceAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var players = await _context.Players
            .Where(p => p.stdStatus == "Active")
            .ToListAsync();

        var attendances = await _context.Attendances
            .Where(a => a.AttendanceDate == today)
            .ToListAsync();

        var result = players.Select(player =>
        {
            var attendance = attendances
                .FirstOrDefault(a => a.PlayerId == player.Id);

            return new TodayAttendanceDto
            {
                PlayerId = player.Id,
                PlayerName = player.stdName,
                AttendanceDate = today,
                Status = attendance?.Status ?? "Absent"
            };
        }).ToList();

        return result;
    }

    public async Task<AttendanceResponseDto> SaveAttendanceAsync(List<MarkAttendanceDto> dto)
    {
        if (dto == null || !dto.Any())
        {
            return new AttendanceResponseDto
            {
                Success = false,
                Message = "No attendance data received."
            };
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        foreach (var item in dto)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a =>
                    a.PlayerId == item.PlayerId &&
                    a.AttendanceDate == today);

            if (attendance == null)
            {
                attendance = new Attendance
                {
                    PlayerId = item.PlayerId,
                    AttendanceDate = today,
                    Status = item.Status,
                    Remarks = item.Remarks,
                    CreatedAT = DateTime.UtcNow
                };

                await _context.Attendances.AddAsync(attendance);
            }
            else
            {
                attendance.Status = item.Status;
                attendance.Remarks = item.Remarks;
                attendance.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();

        return new AttendanceResponseDto
        {
            Success = true,
            Message = "Attendance saved successfully."
        };
    }

    public async Task<List<AdminAttendanceDto>> GetAttendanceHistoryAsync(int? year = null, int? month = null)
    {
        var targetYear = year ?? DateTime.Today.Year;
        var targetMonth = month ?? DateTime.Today.Month;

        var start = new DateOnly(targetYear, targetMonth, 1);
        var end = start.AddMonths(1);

        return await _context.Attendances
            .Include(a => a.Player)
            .Where(a => a.AttendanceDate >= start && a.AttendanceDate < end)
            .OrderByDescending(a => a.AttendanceDate)
            .Select(a => new AdminAttendanceDto
            {
                AttendanceId = a.Id,
                PlayerId = a.PlayerId,
                PlayerName = a.Player.stdName,
                PlayerBelt = a.Player.stdBelt,
                AttendanceDate = a.AttendanceDate,
                Status = a.Status ?? "",
                Remarks = a.Remarks
            })
            .ToListAsync();
    }

    public async Task<List<AttendanceHistoryDto>> GetPlayerAttendanceHistoryAsync(int playerId)
    {
        //I think I don't need this, bcz it's just for chart, and I can set chart through loadAttendance
        return await _context.Attendances
            .Where(a => a.PlayerId == playerId)
            .OrderByDescending(a => a.AttendanceDate)
            .Select(a => new AttendanceHistoryDto
            {
                AttendanceDate = a.AttendanceDate,
                Status = a.Status
            })
            .ToListAsync();
    }



      public async Task<AttendanceResponseDto> UpdateAttendanceAsync(int attendanceId, UpdateAttendanceDto dto)
    {
        var attendance = await _context.Attendances
            .FirstOrDefaultAsync(a => a.Id == attendanceId);

        if (attendance == null)
        {
            return new AttendanceResponseDto
            {
                Success = false,
                Message = "Attendance record not found."
            };
        }

        if (!string.IsNullOrWhiteSpace(dto.Status))
            attendance.Status = dto.Status;

        if (dto.Remarks != null)
            attendance.Remarks = dto.Remarks;

        attendance.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new AttendanceResponseDto
        {
            Success = true,
            Message = "Attendance updated successfully.",
            AttendanceId = attendance.Id,
            Status = attendance.Status ?? ""
        };
    }

    // public async Task<AttendanceResponseDto> CheckInAsync(ClaimsPrincipal user)
    // {
    //     // Console.WriteLine($"Authenticated: {user.Identity?.IsAuthenticated}");
    //     // Console.WriteLine($"Claims Count: {user.Claims.Count()}");
    //     // foreach (var claim in user.Claims)
    //     // {
    //     //     Console.WriteLine($"{claim.Type} = {claim.Value}");
    //     // }    

    //     var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

    //     if (userIdClaim == null)
    //     {
    //         return new AttendanceResponseDto
    //         {
    //             success = false,
    //             message = "User Information Not Found."
    //         };
    //     }

    //     var userId = int.Parse(userIdClaim);

    //     // Check if attendance already exists today
    //     var today = DateOnly.FromDateTime(DateTime.Now);

    //     var existingAttendance = await _context.Attendances
    //         .FirstOrDefaultAsync(a =>
    //             a.UserId == userId &&
    //             a.AttendanceDate == today);

    //     if (existingAttendance != null)
    //         {
    //             return new AttendanceResponseDto
    //             {
    //                 success = false,
    //                 message = "You have already checked in today."
    //             };
    //         }
        
    //       // Create new attendance record
    //     var attendance = new Attendance
    //     {
    //         UserId = userId,
    //         AttendanceDate = DateOnly.FromDateTime(DateTime.Now),
    //         CheckInTime = DateTime.Now,
    //         Status = "Absent",
    //         CreatedAT = DateTime.Now
    //     };

    //     await _context.Attendances.AddAsync(attendance);

    //     await _context.SaveChangesAsync();

    //     return new AttendanceResponseDto
    //     {
    //         success = true,
    //         message = "Checked in successfully.",
    //         CheckInTime = attendance.CheckInTime,
    //         CheckOutTime = attendance.CheckOutTime,
    //         Status = attendance.Status
    //     };

    // }

    //  public async Task<AttendanceResponseDto> CheckOutAsync(ClaimsPrincipal user)
    // {
    //     var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

    //     if(userIdClaim == null)
    //     {
    //         return new AttendanceResponseDto
    //         {
    //             success = false,
    //             message = "User Information Not Found.",
    //         };
    //     }

    //     var userId = int.Parse(userIdClaim);

    //     var today = DateOnly.FromDateTime(DateTime.Now);

    //     var existingAttendance = await _context.Attendances
    //         .FirstOrDefaultAsync(a =>
    //             a.UserId == userId &&
    //             a.AttendanceDate == today);
        
    //     if(existingAttendance == null)
    //     {
    //         return new AttendanceResponseDto
    //         {
    //             success = false,
    //             message = "You haven't checked in today."                
    //         };
    //     }

    //     if(existingAttendance.CheckOutTime != null)
    //     {
    //         return new AttendanceResponseDto
    //         {
    //             success = false,
    //             message = "You have already checked out today."
    //         };
    //     }

    //     existingAttendance.CheckOutTime = DateTime.Now;
    //     existingAttendance.UpdatedAt = DateTime.Now;
    //     existingAttendance.Status = "Present";

    //     await _context.SaveChangesAsync();

    //     return new AttendanceResponseDto
    //     {
    //         success = true,
    //         message = "Checked out successfully.",
    //         CheckInTime = existingAttendance.CheckInTime,
    //         CheckOutTime = existingAttendance.CheckOutTime,
    //         Status = existingAttendance.Status
    //     };

    // }

    // public async Task<List<AttendanceHistoryDto>> GetMyAttendanceAsync(ClaimsPrincipal user)
    // {
    //     var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

    //     if(userIdClaim == null)
    //     {
    //         return new List<AttendanceHistoryDto>();
    //     }

    //     var userId = int.Parse(userIdClaim);

    //     // for filter the attendance
    //     var today = DateOnly.FromDateTime(DateTime.Today);
    //     var firstDayofMonth = new DateOnly(today.Year, today.Month, 1);
    //     var firstDayofNextMonth = firstDayofMonth.AddMonths(1);

    //     return await _context.Attendances
    //                 .Where(a => a.PlayerId == userId
    //                         && a.AttendanceDate >= firstDayofMonth
    //                         && a.AttendanceDate < firstDayofNextMonth)
    //                 .OrderByDescending(a => a.AttendanceDate)
    //                 .Select(a => new AttendanceHistoryDto
    //                 {
    //                     AttendanceDate = a.AttendanceDate,
    //                     // CheckInTime = a.CheckInTime,
    //                     // CheckOutTime = a.CheckOutTime,
    //                     Status = a.Status
    //                 }).ToListAsync();
    // }

    // public async Task<List<AdminAttendanceDto>> GetAllAttendanceAsync()
    // {
    //     // throw new NotImplementedException();
    //     return await _context.Attendances
    //                 .Include(a => a.User)
    //                 .OrderByDescending(a => a.AttendanceDate)
    //                 .Select(a => new AdminAttendanceDto
    //                 {
    //                     AttendanceId = a.Id,
    //                     EmployeeName = a.User.FullName,
    //                     Email = a.User.Email,
    //                     AttendanceDate = a.AttendanceDate,
    //                     CheckInTime = a.CheckInTime,
    //                     CheckOutTime = a.CheckOutTime,
    //                     Status = a.Status,
    //                     Remarks = a.Remarks
    //                 }).ToListAsync();
    // }


    // public async Task<TodayAttendanceDto?> GetTodayAttendanceAsync(ClaimsPrincipal user)
    // {
    //     var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

    //     if (userIdClaim == null)
    //         return null;

    //     var userId = int.Parse(userIdClaim);

    //     var today = DateOnly.FromDateTime(DateTime.Now);

    //     var attendance = await _context.Attendances
    //         .FirstOrDefaultAsync(a =>
    //             a.PlayerId == userId &&
    //             a.AttendanceDate == today);

    //     if (attendance == null)
    //         return null;

    //     return new TodayAttendanceDto
    //     {
    //         AttendanceDate = attendance.AttendanceDate,
    //         // CheckInTime = attendance.CheckInTime,
    //         // CheckOutTime = attendance.CheckOutTime,
    //         Status = attendance.Status,
    //         // IsCheckedIn = attendance.CheckInTime != null,
    //         // IsCheckedOut = attendance.CheckOutTime != null
    //     };
    // }


  

}
