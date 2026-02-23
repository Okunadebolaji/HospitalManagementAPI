using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public PatientController(HospitalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();
            return patient;
        }

      [HttpPost]
public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto dto)
{
    var patient = new Patient
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        DateOfBirth = dto.DateOfBirth ?? DateTime.MinValue,
        Gender = dto.Gender,
        PhoneNumber = dto.PhoneNumber,
        Email = dto.Email,
        Address = dto.Address,
        EmergencyContact = dto.EmergencyContact,
        BloodType = dto.BloodType,
        Allergies = dto.Allergies,
        UserId = dto.UserId
    };

    _context.Patients.Add(patient);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
}

[HttpPost("register-with-user")]
public async Task<IActionResult> RegisterPatientWithUser([FromBody] RegisterPatientWithUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // 🔍 Fetch role from DB using RoleName
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == dto.Role);
        if (role == null)
            return BadRequest("Invalid role specified.");

        // 👤 Create User
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // ✅ Confirm user was saved
        if (user.UserId == 0)
            return StatusCode(500, "User creation failed.");

        // 🏥 Create Patient linked to User
        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            PhoneNumber = dto.PhoneNumber ?? "",
            Email = dto.Email ?? "",
            Address = dto.Address ?? "",
            EmergencyContact = dto.EmergencyContact ?? "",
            BloodType = dto.BloodType ?? "",
            Allergies = dto.Allergies ?? "",
            CreatedAt = DateTime.UtcNow,
            UserId = user.UserId
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return CreatedAtAction(nameof(GetPatientById), new { id = patient.Id }, patient);
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return StatusCode(500, $"Registration failed: {ex.Message}");
    }
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
{
    var existingPatient = await _context.Patients.FindAsync(id);
    if (existingPatient == null)
        return NotFound();

    // ✅ Update fields
    existingPatient.FirstName = updatedPatient.FirstName;
    existingPatient.LastName = updatedPatient.LastName;
    existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
    existingPatient.Gender = updatedPatient.Gender;
    existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
    existingPatient.Email = updatedPatient.Email;
    existingPatient.Address = updatedPatient.Address;
    existingPatient.EmergencyContact = updatedPatient.EmergencyContact;
    existingPatient.BloodType = updatedPatient.BloodType;
    existingPatient.Allergies = updatedPatient.Allergies;

    await _context.SaveChangesAsync();
    return NoContent(); // or return Ok(existingPatient);
}

[HttpGet("by-user/{userId}")]
public IActionResult GetPatientByUserId(int userId)
{
    var patient = _context.Patients.FirstOrDefault(p => p.UserId == userId);
    if (patient == null) return NotFound();
    return Ok(patient);
}


    }
}
