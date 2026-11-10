namespace backend.DTO.Attendance;

public class AttendanceHistoryDto
{
    public DateOnly AttendanceDate { get; set; }

    // public DateTime? CheckInTime { get; set; }

    // public DateTime? CheckOutTime { get; set; }

    public string? Status { get; set; } = string.Empty;    
}

//I think it's for individual and I don't need individual attendace show so we should remove this