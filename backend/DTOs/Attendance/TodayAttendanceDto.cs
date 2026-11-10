namespace backend.DTO.Attendance;

public class TodayAttendanceDto
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = "";
    
    public DateOnly AttendanceDate { get; set; }

    public string? Status { get; set; } = string.Empty;

    // public DateTime? CheckInTime { get; set; }
    // public DateTime? CheckOutTime { get; set; }
    // public bool IsCheckedIn { get; set; }
    // public bool IsCheckedOut { get; set; }
}
