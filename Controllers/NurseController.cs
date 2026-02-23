using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NurseController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public NurseController(HospitalDbContext context)
        {
            _context = context;
        }

        // ✅ GET all nurses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nurse>>> GetNurses()
        {
            return await _context.Nurses.ToListAsync();
        }

        // ✅ GET nurse by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Nurse>> GetNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            return nurse == null ? NotFound() : Ok(nurse);
        }

        // ✅ CREATE nurse
        [HttpPost]
        public async Task<ActionResult<Nurse>> PostNurse(Nurse nurse)
        {
            _context.Nurses.Add(nurse);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNurse), new { id = nurse.NurseId }, nurse);
        }

        // ✅ UPDATE nurse
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNurse(int id, Nurse nurse)
        {
            if (id != nurse.NurseId) return BadRequest();
            _context.Entry(nurse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ DELETE nurse
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null) return NotFound();
            _context.Nurses.Remove(nurse);
            await _context.SaveChangesAsync();
            return NoContent();
        }

         [HttpGet("{nurseId}/image")]
    public async Task<IActionResult> GetNurseImage(int nurseId)
    {
        var image = await _context.NurseProfileImages
            .Where(i => i.NurseId == nurseId && i.IsPrimary)
            .FirstOrDefaultAsync();

        if (image == null || image.ImageData.Length == 0)
            return NotFound("No profile image found.");

        return File(image.ImageData, image.ContentType);
    }

    // ✅ POST nurse profile image
    [HttpPost("{nurseId}/upload-image")]
    public async Task<IActionResult> UploadNurseImage(int nurseId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var imageData = ms.ToArray();

        var existing = await _context.NurseProfileImages
            .Where(i => i.NurseId == nurseId && i.IsPrimary)
            .FirstOrDefaultAsync();

        if (existing != null)
        {
            existing.ImageData = imageData;
            existing.ContentType = file.ContentType;
            existing.FileName = file.FileName;
            existing.UploadedAt = DateTime.Now;
        }
        else
        {
            var newImage = new NurseProfileImage
            {
                NurseId = nurseId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                ImageData = imageData,
                IsPrimary = true,
                UploadedAt = DateTime.Now
            };
            _context.NurseProfileImages.Add(newImage);
        }

        await _context.SaveChangesAsync();
        return Ok("Image uploaded successfully.");
    }
        

        // ✅ ASSIGN nurse to admission + update patient
        [HttpPost("assign")]
        public async Task<IActionResult> AssignNurseToAdmission([FromBody] NurseAssignmentDto dto)
        {
            if (dto == null) return BadRequest("Payload required.");

            var nurseExists = await _context.Nurses.AnyAsync(n => n.NurseId == dto.NurseId);
            if (!nurseExists) return NotFound($"Nurse with id {dto.NurseId} not found.");

            var admission = await _context.PatientAdmissions
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AdmissionId == dto.AdmissionId);

            if (admission == null) return NotFound($"Admission with id {dto.AdmissionId} not found.");

            var duplicate = await _context.NurseAssignments
                .AnyAsync(na => na.NurseId == dto.NurseId && na.AdmissionId == dto.AdmissionId);
            if (duplicate) return Conflict("This nurse is already assigned to that admission.");

            var assignment = new NurseAssignment
            {
                NurseId = dto.NurseId,
                AdmissionId = dto.AdmissionId
            };

            _context.NurseAssignments.Add(assignment);

            // ✅ Update Patient table
            if (admission.Patient != null)
            {
                admission.Patient.NurseId = dto.NurseId;
                _context.Patients.Update(admission.Patient);
            }

            await _context.SaveChangesAsync();

            return Ok(assignment);
        }

        // ✅ GET all nurse assignments
        [HttpGet("assignments")]
        public async Task<IActionResult> GetAssignments()
        {
            var list = await _context.NurseAssignments
                .Include(n => n.Nurse)
                .Include(n => n.Admission)
                    .ThenInclude(a => a.Patient)
                .ToListAsync();

            return Ok(list);
        }

        // ✅ GET assignments for a specific nurse
        [HttpGet("{id}/assignments")]
        public async Task<IActionResult> GetAssignmentsForNurse(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse == null)
                return NotFound($"Nurse with ID {id} not found.");

            var assignments = await _context.NurseAssignments
                .Include(n => n.Admission)
                    .ThenInclude(a => a.Patient)
                .Where(n => n.NurseId == id)
                .ToListAsync();

            return Ok(assignments);
        }
    }
}
