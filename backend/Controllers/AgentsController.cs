using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BonusTrackerApi.Data;
using BonusTrackerApi.Models;

namespace BonusTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentsController(AppDbContext db) : ControllerBase
{
    [HttpGet("{team}")]
    public async Task<IActionResult> GetByTeam(string team)
    {
        var agents = await db.Agents
            .Where(a => a.Team == team.ToUpper())
            .OrderBy(a => a.Id)
            .ToListAsync();
        return Ok(agents);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateName(int id, [FromBody] UpdateNameRequest req)
    {
        var agent = await db.Agents.FindAsync(id);
        if (agent == null) return NotFound();
        agent.Name = req.Name;
        await db.SaveChangesAsync();
        return Ok(agent);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAgentRequest req)
    {
        var agent = new Agent { Name = req.Name, Team = req.Team.ToUpper() };
        db.Agents.Add(agent);
        await db.SaveChangesAsync();
        return Ok(agent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var agent = await db.Agents.FindAsync(id);
        if (agent == null) return NotFound();
        db.Agents.Remove(agent);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

public record UpdateNameRequest(string Name);
public record CreateAgentRequest(string Name, string Team);
