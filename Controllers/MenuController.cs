using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;



[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public MenuController(HospitalDbContext context)
    {
        _context = context;
    }

   [HttpGet("{userId}")]
public async Task<IActionResult> GetMenuByUserId(int userId)
{
    var user = await _context.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.UserId == userId);

    if (user == null) return NotFound("User not found");

    var menuItems = await _context.MenuPermissions
        .Where(mp => mp.RoleId == user.RoleId && mp.CanView)
        .Include(mp => mp.Menu)
        .Select(mp => new {
            mp.Menu!.Name,
            mp.Menu.Route,
            mp.CanEdit,
            mp.CanDelete
        })
        .ToListAsync();

    return Ok(new {
        userId = user.UserId,
        roleName = user.Role!.RoleName,
        menuItems = menuItems
    });
}

}
