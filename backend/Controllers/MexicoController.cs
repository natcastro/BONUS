using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonusTrackerApi.Data;
using BonusTrackerApi.Models;

namespace BonusTrackerApi.Controllers;

[ApiController]
[Route("api/mex")]
public class MexicoController(AppDbContext db) : ControllerBase
{
    // ── Attendance ────────────────────────────────────────────────────────────

    [HttpGet("attendance/{year}/{month}")]
    public async Task<IActionResult> GetAttendance(int year, int month)
    {
        var data = await db.MexAttendances
            .Include(a => a.Agent)
            .Where(a => a.Year == year && a.Month == month)
            .ToListAsync();
        return Ok(data);
    }

    [HttpPut("attendance")]
    public async Task<IActionResult> UpsertAttendance([FromBody] MexAttendance input)
    {
        var existing = await db.MexAttendances
            .FirstOrDefaultAsync(a => a.AgentId == input.AgentId && a.Year == input.Year && a.Month == input.Month);

        if (existing == null)
            db.MexAttendances.Add(input);
        else
            existing.Status = input.Status;

        await db.SaveChangesAsync();
        return Ok();
    }

    // ── Live Sales ────────────────────────────────────────────────────────────

    [HttpGet("sales/{year}/{month}")]
    public async Task<IActionResult> GetSales(int year, int month)
    {
        var sales = await db.MexLiveSales
            .Include(s => s.Agent)
            .Where(s => s.Year == year && s.Month == month)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
        return Ok(sales);
    }

    [HttpPost("sales")]
    public async Task<IActionResult> AddSale([FromBody] MexLiveSale sale)
    {
        db.MexLiveSales.Add(sale);
        await db.SaveChangesAsync();
        return Ok(sale);
    }

    [HttpDelete("sales/{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        var sale = await db.MexLiveSales.FindAsync(id);
        if (sale == null) return NotFound();
        db.MexLiveSales.Remove(sale);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Monthly Goal ──────────────────────────────────────────────────────────

    [HttpGet("goal/{year}/{month}")]
    public async Task<IActionResult> GetGoal(int year, int month)
    {
        var goal = await db.MexMonthlyGoals
            .FirstOrDefaultAsync(g => g.Year == year && g.Month == month);
        return Ok(goal);
    }

    [HttpPut("goal")]
    public async Task<IActionResult> UpsertGoal([FromBody] MexMonthlyGoal input)
    {
        var existing = await db.MexMonthlyGoals
            .FirstOrDefaultAsync(g => g.Year == input.Year && g.Month == input.Month);

        if (existing == null)
            db.MexMonthlyGoals.Add(input);
        else
        {
            existing.GoalAmount = input.GoalAmount;
            existing.ActualAmount = input.ActualAmount;
        }

        await db.SaveChangesAsync();
        return Ok();
    }
}
