namespace BonusTrackerApi.Models;

public class Appeal
{
    public int Id { get; set; }
    public int AgentId { get; set; }
    public Agent? Agent { get; set; }
    public string Date { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty; // "Amazon" | "TikTok"
    public string Status { get; set; } = string.Empty;   // "inProgress" | "completed"
    public string Outcome { get; set; } = string.Empty;  // "fullRefund" | "partialRefund" | "fee" | "lost"
    public int Year { get; set; }
    public string CycleId { get; set; } = string.Empty;  // e.g. "0_1"
}
