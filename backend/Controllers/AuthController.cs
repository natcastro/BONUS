using Microsoft.AspNetCore.Mvc;

namespace BonusTrackerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string UsaPassword = "usa2026";
    private const string MexPassword = "mex2026";

    [HttpPost("verify")]
    public IActionResult Verify([FromBody] VerifyRequest req)
    {
        var expected = req.Team.ToUpper() switch
        {
            "USA" => UsaPassword,
            "MEX" => MexPassword,
            _ => null
        };

        if (expected == null || req.Password != expected)
            return Unauthorized(new { message = "Incorrect password." });

        return Ok(new { team = req.Team.ToUpper() });
    }
}

public record VerifyRequest(string Team, string Password);
