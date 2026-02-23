using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;
using HospitalManagementAPI.Data;
using BCrypt.Net;

[ApiController]
[Route("api/[controller]")]
public class AuthoController : ControllerBase
{ 
    private readonly IJwtService _jwt;
    private readonly HospitalDbContext _context;

    public AuthoController(HospitalDbContext context, IJwtService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _jwt.GenerateToken(user);

        var menus = await _context.MenuPermissions
            .Where(mp => mp.RoleId == user.RoleId && mp.CanView)
            .Select(mp => new {
                mp.Menu!.Name,
                mp.Menu.Route,
                mp.CanEdit,
                mp.CanDelete
            })
            .ToListAsync();

        return Ok(new {
            token = token,
            userId = user.UserId,
            name = user.Username,
            email = user.Email,
            role = user.Role!.RoleName,
            menu = menus.Select(m => new {
                name = m.Name,
                route = m.Route,
                canEdit = m.CanEdit,
                canDelete = m.CanDelete
            }).ToList()
        });
    }
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterRequest request)
{
    if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        return BadRequest("Username already exists");

    var user = new User
    {
        Username = request.Username,
        Email = request.Email,
        RoleId = request.RoleId,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        CreatedAt = DateTime.UtcNow,
        IsActive = true,
        CreatedBy = request.CreatedBy
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.UserId == user.UserId);

    var token = _jwt.GenerateToken(user!);

    var menus = await _context.MenuPermissions
        .Where(mp => mp.RoleId == user!.RoleId && mp.CanView)
        .Select(mp => new {
            mp.Menu!.Name,
            mp.Menu.Route,
            mp.CanEdit,
            mp.CanDelete
        })
        .ToListAsync();

    return Ok(new {
        token = token,
        userId = user!.UserId,
        name = user.Username,
        email = user.Email,
        role = user.Role!.RoleName,
        menu = menus.Select(m => new {
            name = m.Name,
            route = m.Route,
            canEdit = m.CanEdit,
            canDelete = m.CanDelete
        }).ToList()
    });
}


}
