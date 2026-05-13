namespace BonusTrackerApi.Models;

public class MexLiveSale
{
    public int Id { get; set; }
    public int AgentId { get; set; }
    public Agent? Agent { get; set; }
    public string Date { get; set; } = string.Empty;
    public decimal SalesAmount { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
