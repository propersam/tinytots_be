using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Tinytots.DbContext;
using Tinytots.DTO;
using Tinytots.Models;

namespace Tinytots.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TinytotsDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(TinytotsDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            AdminUser? user = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            string token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(8),
                User = new
                {
                    user.AdminUserId,
                    user.Email,
                    user.Name
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    // ─────────────────────────────────────────────
    // POST /api/auth/logout
    // Admin. JWT required. (Stateless — client drops token.)
    // ─────────────────────────────────────────────
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Logged out successfully" });
    }

    // ─────────────────────────────────────────────
    // GET /api/auth/me
    // Admin. JWT required. Returns current admin profile.
    // ─────────────────────────────────────────────
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        try
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Invalid token");
            }

            AdminUser? user = await _context.AdminUsers.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(new
            {
                user.AdminUserId,
                user.Email,
                user.Name
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    // ─────────────────────────────────────────────
    // Private: JWT generation helper
    // Uses JsonWebTokenHandler (IdentityModel 7+ modern API)
    // Replaces legacy JwtSecurityTokenHandler
    // ─────────────────────────────────────────────
    private string GenerateJwtToken(AdminUser user)
    {
        string? jwtKey = _configuration["Jwt:Key"];
        string? jwtIssuer = _configuration["Jwt:Issuer"];
        string? jwtAudience = _configuration["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT key is not configured.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.AdminUserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = credentials
        };

        // JsonWebTokenHandler is the modern replacement for JwtSecurityTokenHandler
        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }
}
