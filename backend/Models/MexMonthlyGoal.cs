namespace BonusTrackerApi.Models;

public class MexMonthlyGoal
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal GoalAmount { get; set; }
    public decimal ActualAmount { get; set; }
}
