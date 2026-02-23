using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using HospitalManagementAPI.Dtos;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public AppointmentsController(HospitalDbContext context)
    {
        _context = context;
    }

[HttpGet("appointments/today")]
public IActionResult GetTodaysAppointments()
{
    var today = DateTime.Today;
    var appointments = _context.Appointments
        .Where(a => a.AppointmentDate.Date == today)
        .ToList();

    return Ok(appointments);
}




    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingAppointments()
    {
        var now = DateTime.Now;
var rawAppointments = await _context.Appointments
    // .Where(a => a.AppointmentDate >= now)
    .OrderBy(a => a.AppointmentDate)
    .Include(a => a.Patient)
    .Include(a => a.Doctor)
    .ToListAsync(); 

var appointments = rawAppointments.Select(a => new {
    a.AppointmentDate,
    a.Type,
    a.Fee,
    a.Notes,
    PatientName = a.Patient!.FirstName + " " + a.Patient.LastName,
    DoctorName = a.Doctor!.Name
});


        return Ok(appointments);
    }

 [HttpGet("appointments")]
public async Task<IActionResult> GetAppointments()
{
    var rawAppointments = await _context.Appointments
        .Include(a => a.Patient)
        .Include(a => a.Doctor) 
        .Select(a => new {
            a.AppointmentId,
            FirstName = a.Patient!.FirstName,
            LastName = a.Patient.LastName,
            Gender = a.Patient.Gender,
            DateOfBirth = a.Patient.DateOfBirth,
            AppointmentDate = a.AppointmentDate,
            Type = a.Type,
            Notes = a.Notes,
            Status = a.Status,
            DoctorName = a.Doctor!.Name 
        }).ToListAsync();

    var appointments = rawAppointments.Select(a => new AppointmentListDto
    {
        AppointmentId = a.AppointmentId,
        PatientName = a.FirstName + " " + a.LastName,
        Gender = a.Gender!,
        Age = CalculateAge(a.DateOfBirth),
        AppointmentDate = a.AppointmentDate,
        Type = a.Type,
        Diagnosis = a.Notes,
        Status = a.Status, 
        DoctorName = a.DoctorName! 
    }).ToList();

    return Ok(appointments);
}

private int CalculateAge(DateTime dob)
{
    var today = DateTime.Today;
    var age = today.Year - dob.Year;
    if (dob.Date > today.AddYears(-age)) age--;
    return age;
}



    [HttpPost]
public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var appointment = new Appointment
    {
        DoctorId = dto.DoctorId,
        PatientId = dto.PatientId,
        AppointmentDate = dto.AppointmentDate,
        Type = dto.Type,
        Fee = dto.Fee,
        Notes = dto.Notes,
        Status = "Pending"
    };
      var doctorExists = await _context.Doctors.AnyAsync(d => d.DoctorId == dto.DoctorId);
if (!doctorExists)
{
    return BadRequest("Invalid DoctorId. Doctor does not exist.");
}

    _context.Appointments.Add(appointment);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetUpcomingAppointments), new { id = appointment.AppointmentId }, appointment);
}

[HttpGet("top-treatments")]
public async Task<IActionResult> GetTopTreatments()
{
    var treatments = await _context.Appointments
        .Where(a => a.Type != null)
        .GroupBy(a => a.Type)
        .Select(g => new {
            Treatment = g.Key,
            Count = g.Count()
        })
        .OrderByDescending(g => g.Count)
        .Take(5)
        .ToListAsync();

    return Ok(treatments);
}

[HttpPut("{id}/confirm")]

public async Task<IActionResult> ConfirmAppointment(int id)
{
    var appointment = await _context.Appointments.FindAsync(id);
    if (appointment == null)
    {
        return NotFound("Appointment not found.");
    }

    appointment.Status = "Confirmed";
    await _context.SaveChangesAsync();

    return NoContent();
}



}
