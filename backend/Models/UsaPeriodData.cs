namespace BonusTrackerApi.Models;

public class UsaPeriodData
{
    public int Id { get; set; }
    public int AgentId { get; set; }
    public Agent? Agent { get; set; }
    public int Year { get; set; }
    public string CycleId { get; set; } = string.Empty;
    public string AmazonHealth { get; set; } = "bad";  // "good" | "minor" | "bad"
    public string CsQuality { get; set; } = "2";       // "0" | "1" | "2"
}
