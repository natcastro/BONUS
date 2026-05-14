namespace BonusTrackerApi.Models;

public class TikTokScore
{
    public int Id { get; set; }
    public string Date { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public int Duration { get; set; } = 1; // 1 = single day, 7 = full week
    public int Year { get; set; }
    public string CycleId { get; set; } = string.Empty;
}
