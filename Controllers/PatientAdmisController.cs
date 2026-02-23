using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;

namespace HospitalManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientAdmisController : ControllerBase
    {
        private readonly HospitalDbContext _context;

        public PatientAdmisController(HospitalDbContext context)
        {
            _context = context;
        }

        // ✅ GET all admissions with patient, doctor, and nurse assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientAdmission>>> GetAllAdmissions()
        {
            var admissions = await _context.PatientAdmissions
                .Include(pa => pa.Patient)
                .Include(pa => pa.Doctor)
                .Include(pa => pa.NurseAssignments)
                    .ThenInclude(na => na.Nurse)
                .ToListAsync();

            return Ok(admissions);
        }

        // ✅ GET admission by ID with full details
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientAdmission>> GetAdmissionById(int id)
        {
            var admission = await _context.PatientAdmissions
                .Include(pa => pa.Patient)
                .Include(pa => pa.Doctor)
                .Include(pa => pa.NurseAssignments)
                    .ThenInclude(na => na.Nurse)
                .FirstOrDefaultAsync(pa => pa.AdmissionId == id);

            if (admission == null)
                return NotFound();

            return Ok(admission);
        }

        // ✅ CREATE a new admission
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmission([FromBody] CreateAdmissionDto dto)
        {
            // Validate input
            if (dto.PatientId <= 0 || dto.DoctorId <= 0 || string.IsNullOrWhiteSpace(dto.ReasonForAdmission))
            {
                return BadRequest("Missing or invalid admission data.");
            }

            // Map DTO to entity
            var admission = new PatientAdmission
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AdmissionDate = dto.AdmissionDate,
                DischargeDate = dto.DischargeDate,
                ReasonForAdmission = dto.ReasonForAdmission,
                RoomNumber = dto.RoomNumber
            };

            // Save to DB
            _context.PatientAdmissions.Add(admission);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            var savedAdmission = await _context.PatientAdmissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.NurseAssignments)
                    .ThenInclude(na => na.Nurse)
                .FirstOrDefaultAsync(a => a.AdmissionId == admission.AdmissionId);

            return CreatedAtAction(nameof(GetAdmissionById), new { id = savedAdmission!.AdmissionId }, savedAdmission);
        }
    }
}
