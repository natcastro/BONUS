namespace BonusTrackerApi.Models;

public class Agent
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty; // "USA" or "MEX"
}
