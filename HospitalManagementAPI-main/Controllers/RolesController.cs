using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;


[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public RolesController(HospitalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Select(r => new { r.RoleId, r.RoleName })
            .ToListAsync();

        return Ok(roles);
    }
[HttpGet("{id}")]
public async Task<IActionResult> GetRoleById(int id)
{
    var role = await _context.Roles.FindAsync(id);
    if (role == null)
        return NotFound("Role not found");

    return Ok(role);
}

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName))
            return BadRequest("Role name is required");

        var role = new Role { RoleName = dto.RoleName };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
    }
}
