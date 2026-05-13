namespace BonusTrackerApi.Models;

public class MexAttendance
{
    public int Id { get; set; }
    public int AgentId { get; set; }
    public Agent? Agent { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string Status { get; set; } = "multiple"; // "none" | "justified" | "multiple"
}
