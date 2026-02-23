using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public DoctorController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors()
        {
            return await _context.Doctors
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    UserId = d.UserId,
                    RoleId = d.RoleId,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    Phone = d.Phone,
                    Email = d.Email
                })
                .ToListAsync();
        }

        // GET: api/Doctor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDto>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors
                .Where(d => d.DoctorId == id)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    UserId = d.UserId,
                    RoleId = d.RoleId,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    Phone = d.Phone,
                    Email = d.Email
                })
                .FirstOrDefaultAsync();

            if (doctor == null)
                return NotFound();

            return doctor;
        }

        // GET: api/Doctor/with-admissions
        [HttpGet("with-admissions")]
        public async Task<ActionResult<IEnumerable<DoctorWithAdmissionsDto>>> GetDoctorsWithAdmissions()
        {
            var doctors = await _context.Doctors
                .Include(d => d.PatientAdmissions)
                    .ThenInclude(pa => pa.Patient)
                .Select(d => new DoctorWithAdmissionsDto
                {
                    DoctorId = d.DoctorId,
                    UserId = d.UserId,
                    RoleId = d.RoleId,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    Admissions = d.PatientAdmissions.Select(pa => new AdmissionDto
                    {
                        AdmissionId = pa.AdmissionId,
                        PatientName = pa.Patient.FirstName + " " + pa.Patient.LastName,
                        Reason = pa.ReasonForAdmission,
                        Room = pa.RoomNumber,
                        Date = pa.AdmissionDate ?? DateTime.MinValue
                    }).ToList()
                })
                .ToListAsync();

            return Ok(doctors);
        }

        // GET: api/Doctor/with-admissions/{id}
        [HttpGet("with-admissions/{id}")]
        public async Task<ActionResult<DoctorWithAdmissionsDto>> GetDoctorWithAdmissionsById(int id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.PatientAdmissions)
                    .ThenInclude(pa => pa.Patient)
                .Where(d => d.DoctorId == id)
                .Select(d => new DoctorWithAdmissionsDto
                {
                    DoctorId = d.DoctorId,
                    UserId = d.UserId,
                    RoleId = d.RoleId,
                    Name = d.Name,
                    Specialty = d.Specialty,
                    Admissions = d.PatientAdmissions.Select(pa => new AdmissionDto
                    {
                        AdmissionId = pa.AdmissionId,
                        PatientName = pa.Patient.FirstName + " " + pa.Patient.LastName,
                        Reason = pa.ReasonForAdmission,
                        Room = pa.RoomNumber,
                        Date = pa.AdmissionDate ?? DateTime.MinValue
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

[HttpGet("user/{userId}")]
public async Task<IActionResult> GetDoctorByUserId(int userId)
{
    var doctor = await _context.Doctors
        .FirstOrDefaultAsync(d => d.UserId == userId);

    if (doctor == null)
        return NotFound("Doctor profile not found for this user.");

    return Ok(doctor);
}

        // POST: api/Doctor
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDto dto)
        {
            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role == null)
                return BadRequest("Invalid RoleId.");

            if (!string.Equals(dto.Specialty, role.RoleName, StringComparison.OrdinalIgnoreCase))
                return BadRequest($"Specialty must match the role name: {role.RoleName}");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return BadRequest("UserId does not exist. Please register the user first.");

            var existingDoctor = await _context.Doctors
                .FirstOrDefaultAsync(d => d.UserId == dto.UserId);

            if (existingDoctor != null)
                return BadRequest("Doctor profile already exists for this user.");

            var doctor = new Doctor
            {
                Name = dto.Name,
                Specialty = dto.Specialty,
                Phone = dto.Phone,
                Email = dto.Email,
                RoleId = dto.RoleId,
                UserId = dto.UserId,
                JoinDate = DateTime.UtcNow
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(doctor);
        }

[HttpGet("doctor/{doctorId}/by-status")]
public async Task<IActionResult> GetAppointmentsByDoctorAndStatus(int doctorId, [FromQuery] string status)
{
    var appointments = await _context.Appointments
        .Where(a => a.DoctorId == doctorId && a.Status == status)
        .Include(a => a.Patient)
        .OrderBy(a => a.AppointmentDate)
        .Select(a => new {
            a.AppointmentId,
            a.Type,
            a.AppointmentDate,
            a.Status,
            PatientName = a.Patient!.FirstName + " " + a.Patient.LastName
        })
        .ToListAsync();

    return Ok(appointments);
}

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
                return BadRequest();

            var role = await _context.Roles.FindAsync(doctor.RoleId);
            if (role == null)
                return BadRequest("Invalid RoleId.");

            if (!string.Equals(doctor.Specialty, role.RoleName, StringComparison.OrdinalIgnoreCase))
                return BadRequest($"Specialty must match the role name: {role.RoleName}");

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Doctors.Any(d => d.DoctorId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost("{userId}/profile")]
        public async Task<IActionResult> SaveDoctorProfile(int userId, [FromBody] DoctorProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var role = await _context.Roles.FindAsync(user.RoleId);
            if (role == null)
                return BadRequest("User has no valid role assigned");

            if (!string.Equals(dto.Specialty, role.RoleName, StringComparison.OrdinalIgnoreCase))
                return BadRequest($"Specialty must match the role name: {role.RoleName}");

            var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
            if (existingDoctor != null)
                return BadRequest("Doctor profile already exists for this user");

            var doctor = new Doctor
            {
                UserId = userId,
                Name = dto.Name,
                Specialty = dto.Specialty,
                Phone = dto.Phone,
                Email = dto.Email,
                RoleId = user.RoleId
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            // ✅ Return JSON instead of plain text
            return Ok(new { message = "Doctor profile saved" });
        }



        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Doctor/{doctorId}/profile-image
        [HttpPost("{doctorId}/profile-image")]
        public async Task<IActionResult> UploadProfileImage(int doctorId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Empty file.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var existing = await _context.DoctorProfileImages
                .FirstOrDefaultAsync(i => i.DoctorId == doctorId && i.IsPrimary == true);

            if (existing != null)
                existing.IsPrimary = false;

            var image = new DoctorProfileImage
            {
                DoctorId = doctorId,
                FileName = file.FileName,
                ContentType = file.ContentType!,
                ImageData = ms.ToArray(),
                IsPrimary = true
            };

            _context.DoctorProfileImages.Add(image);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/Doctor/{doctorId}/profile-image
        [HttpGet("{doctorId}/profile-image")]
        public async Task<IActionResult> GetProfileImage(int doctorId)
        {
            if (doctorId <= 0)
                return BadRequest("Invalid doctor ID.");

            var image = await _context.DoctorProfileImages
                .Where(i => i.DoctorId == doctorId && i.IsPrimary == true)
                .FirstOrDefaultAsync();

            if (image == null)
                return NotFound();

            Response.Headers["Cache-Control"] = "public,max-age=86400";

            return File(image.ImageData, image.ContentType, image.FileName);
        }
[HttpPost("bulk-upload")]
public async Task<IActionResult> BulkUploadDoctors([FromBody] List<StaffCreateDto> doctors)
{
    if (doctors == null || !doctors.Any())
        return BadRequest("No doctor data provided.");

    var saved = new List<string>();
    var failed = new List<string>();
Console.WriteLine("Incoming payload:");
    foreach (var dto in doctors)
    {
        try
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Specialty) ||
                string.IsNullOrWhiteSpace(dto.Phone) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                Console.WriteLine($"Validation failed for {dto.Name}");
                failed.Add(dto.Name ?? "Unknown");
                continue;
            }

            // Check for duplicate email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
            {
                Console.WriteLine($"Duplicate email for {dto.Email}");
                failed.Add(dto.Name ?? "Duplicate Email");
                continue;
            }

            // ✅ Create User
            var user = new User
            {
                Username = dto.Email,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                RoleId = dto.RoleId != 0 ? dto.RoleId : 2,
                CreatedAt = DateTime.Now,
                IsActive = true,
                CreatedBy = dto.CreatedBy ?? 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Created User: {user.UserId} for {user.Email}");

            // ✅ Create Doctor
            var doctor = new Doctor
            {
                Name = dto.Name,
                Specialty = dto.Specialty,
                Phone = dto.Phone,
                Email = dto.Email,
                Bio = dto.Bio,
                JoinDate = dto.JoinDate != default ? dto.JoinDate : DateTime.Now,
                RoleId = user.RoleId,
                UserId = user.UserId
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Created Doctor: {doctor.DoctorId} for {doctor.Name}");

            saved.Add(dto.Name);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving {dto.Name}: {ex.Message}");
            failed.Add(dto.Name ?? "Unknown");
        }
    }

    Console.WriteLine("Bulk upload triggered with " + doctors.Count + " entries.");

    return Ok(new
    {
        message = "Bulk upload completed.",
        savedCount = saved.Count,
        failedCount = failed.Count,
        saved,
        failed
    });
}


private string HashPassword(string password)
{
    var hasher = new PasswordHasher<User>();
    return hasher.HashPassword(null!, password);
}




    }//





}

