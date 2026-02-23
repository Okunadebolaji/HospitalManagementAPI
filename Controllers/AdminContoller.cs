using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;
using System.Security.Claims;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public AdminController(HospitalDbContext context)
    {
        _context = context;
    }

    [HttpPost("create-staff")]
    public async Task<IActionResult> CreateStaff([FromBody] StaffCreateDto dto)
    {
        if (_context.Users.Any(u => u.Email == dto.Email))
            return BadRequest("Email already exists.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out var adminId))
            return Unauthorized("Invalid admin ID.");

        var role = await _context.Roles.FindAsync(dto.RoleId);
        if (role == null)
            return BadRequest("Invalid role ID.");

        var user = new User
        {
            Username = dto.Email,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? "Default123!"),
            RoleId = dto.RoleId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            CreatedBy = adminId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        switch (role.RoleName.ToLower())
        {
            case "doctor":
            case "consultant":
            case "gynaecologist":
                var doctor = new Doctor
                {
                    Name = dto.Name,
                    Specialty = dto.Specialty,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    RoleId = dto.RoleId,
                    UserId = user.UserId,
                    Bio = dto.Bio,
                    JoinDate = dto.JoinDate
                };
                _context.Doctors.Add(doctor);
                break;

            case "nurse":
                var nurse = new Nurse
                {
                    Name = dto.Name,
                    Specialty = dto.Specialty,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    RolesId = dto.RoleId,
                    UserId = user.UserId
                };
                _context.Nurses.Add(nurse);
                break;

             case "patient":
    var patient = new Patient
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        PhoneNumber = dto.Phone,
        Gender = dto.Gender,
        DateOfBirth = dto.DateOfBirth,
        Address = dto.Address,
        UserId = user.UserId
    };
    _context.Patients.Add(patient);
    break;


           

            default:
                return BadRequest("Unsupported role for staff creation.");
        }

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Staff created successfully",
            user = new
            {
                user.UserId,
                user.Email,
                role.RoleName,
                dto.Name,
                dto.Specialty
            }
        });
    }
}
