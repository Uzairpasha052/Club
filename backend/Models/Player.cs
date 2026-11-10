namespace backend.Models;

public class Player
{
    
    public int Id { get; set; }

    public string stdName { get; set; } = string.Empty;

    public string stdFName { get; set; } = string.Empty;

    public string stdAge { get; set; } = string.Empty;

    public string stdGender { get; set; } = string.Empty;
    
    public string stdContact { get; set; } = string.Empty;

    public string stdBelt { get; set; } = string.Empty;

    public string stdHistory { get; set; } = "FreshStudent"; 

    public string stdReason { get; set; } = string.Empty;

    public string stdFee { get; set; } = string.Empty;

    public string stdStatus { get; set; } = "Active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public ICollection<CashIn> CashIns { get; set; } = new List<CashIn>();

}
