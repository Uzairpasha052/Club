namespace backend.DTOs.Player;

public class CreatePlayerDto
{
    public string stdName { get; set; } = string.Empty;

    public string stdFName { get; set; } = string.Empty;

    public string stdAge { get; set; } = string.Empty;

    public string stdGender { get; set; } = string.Empty;

    public string stdContact { get; set; } = string.Empty;

    public string stdBelt { get; set; } = "White";

    public string stdHistory { get; set; } = "FreshStudent";

    public string stdReason { get; set; } = string.Empty;

    public string stdFee { get; set; } = string.Empty;

    public string stdStatus { get; set; } = "Active";
}