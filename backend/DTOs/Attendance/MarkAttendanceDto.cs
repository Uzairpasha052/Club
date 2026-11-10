namespace backend.DTO.Attendance;

public class MarkAttendanceDto
{
    public int PlayerId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? Remarks { get; set; }
}
