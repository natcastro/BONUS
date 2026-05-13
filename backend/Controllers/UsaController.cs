using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonusTrackerApi.Data;
using BonusTrackerApi.Models;

namespace BonusTrackerApi.Controllers;

[ApiController]
[Route("api/usa")]
public class UsaController(AppDbContext db) : ControllerBase
{
    // ── Appeals ──────────────────────────────────────────────────────────────

    [HttpGet("appeals/{year}/{cycleId}")]
    public async Task<IActionResult> GetAppeals(int year, string cycleId)
    {
        var appeals = await db.Appeals
            .Include(a => a.Agent)
            .Where(a => a.Year == year && a.CycleId == cycleId && a.Agent.Team == "USA")
            .OrderByDescending(a => a.Date)
            .ToListAsync();
        return Ok(appeals);
    }

    [HttpPost("appeals")]
    public async Task<IActionResult> AddAppeal([FromBody] Appeal appeal)
    {
        db.Appeals.Add(appeal);
        await db.SaveChangesAsync();
        return Ok(appeal);
    }

    [HttpPut("appeals/{id}")]
    public async Task<IActionResult> UpdateAppeal(int id, [FromBody] Appeal updated)
    {
        var appeal = await db.Appeals.FindAsync(id);
        if (appeal == null) return NotFound();
        appeal.AgentId = updated.AgentId;
        appeal.Date = updated.Date;
        appeal.OrderNumber = updated.OrderNumber;
        appeal.Platform = updated.Platform;
        appeal.Status = updated.Status;
        appeal.Outcome = updated.Outcome;
        await db.SaveChangesAsync();
        return Ok(appeal);
    }

    [HttpDelete("appeals/{id}")]
    public async Task<IActionResult> DeleteAppeal(int id)
    {
        var appeal = await db.Appeals.FindAsync(id);
        if (appeal == null) return NotFound();
        db.Appeals.Remove(appeal);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // ── Period Data (Amazon Health + CS Quality) ──────────────────────────

    [HttpGet("period/{year}/{cycleId}")]
    public async Task<IActionResult> GetPeriodData(int year, string cycleId)
    {
        var agents = await db.Agents.Where(a => a.Team == "USA").ToListAsync();
        var data = await db.UsaPeriodData
            .Where(p => p.Year == year && p.CycleId == cycleId)
            .Include(p => p.Agent)
            .ToListAsync();
        return Ok(data);
    }

    [HttpPut("period")]
    public async Task<IActionResult> UpsertPeriodData([FromBody] UsaPeriodData input)
    {
        var existing = await db.UsaPeriodData
            .FirstOrDefaultAsync(p => p.AgentId == input.AgentId && p.Year == input.Year && p.CycleId == input.CycleId);

        if (existing == null)
        {
            db.UsaPeriodData.Add(input);
        }
        else
        {
            existing.AmazonHealth = input.AmazonHealth;
            existing.CsQuality = input.CsQuality;
        }
        await db.SaveChangesAsync();
        return Ok();
    }

    // ── TikTok Scores ────────────────────────────────────────────────────────

    [HttpGet("tiktok/{year}/{cycleId}")]
    public async Task<IActionResult> GetTikTok(int year, string cycleId)
    {
        var scores = await db.TikTokScores
            .Where(t => t.Year == year && t.CycleId == cycleId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
        return Ok(scores);
    }

    [HttpPost("tiktok")]
    public async Task<IActionResult> AddTikTok([FromBody] TikTokScore score)
    {
        db.TikTokScores.Add(score);
        await db.SaveChangesAsync();
        return Ok(score);
    }

    [HttpDelete("tiktok/{id}")]
    public async Task<IActionResult> DeleteTikTok(int id)
    {
        var score = await db.TikTokScores.FindAsync(id);
        if (score == null) return NotFound();
        db.TikTokScores.Remove(score);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
