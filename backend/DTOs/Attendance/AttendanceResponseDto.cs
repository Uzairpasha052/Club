namespace backend.DTO.Attendance;

public class AttendanceResponseDto
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public int? AttendanceId { get; set; }

    public string Status { get; set; } = string.Empty;
}
