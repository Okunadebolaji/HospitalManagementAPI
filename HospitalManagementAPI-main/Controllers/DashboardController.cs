using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementAPI.Data;
using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Dtos;

[ApiController]
[Route("api/admin")]

public class DashboardController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public DashboardController(HospitalDbContext context)
    {
        _context = context;
    }

    [HttpGet("dashboard-metrics")]
public async Task<IActionResult> GetDashboardMetrics()
{
  var now = DateTime.UtcNow;
    var today = now.Date;
    var tomorrow = today.AddDays(1);

    var startOfMonth = new DateTime(now.Year, now.Month, 1);
    var startOfLastMonth = startOfMonth.AddMonths(-1);

  var totalPatientsThisMonth = await _context.Patients
    .Where(p => p.CreatedAt >= startOfMonth)
    .CountAsync();


    var totalPatientsAllTime = await _context.Patients.CountAsync();

    var consultations = await _context.Appointments
        .Where(a => a.Type == "Consultation" && a.AppointmentDate >= startOfMonth)
        .CountAsync();

    var procedures = await _context.Appointments
        .Where(a => a.Type == "Procedure" && a.AppointmentDate >= startOfMonth)
        .CountAsync();

        var payments = await _context.Appointments
            .Where(a => a.AppointmentDate >= startOfMonth)
            .SumAsync(a => a.Fee) ?? 0 ;

    var previousPayments = await _context.Appointments
        .Where(a => a.AppointmentDate >= startOfLastMonth && a.AppointmentDate < startOfMonth)
        .SumAsync(a => a.Fee)?? 0 ;

   

var appointmentsToday = await _context.Appointments
    .Where(a => a.AppointmentDate >= today && a.AppointmentDate < tomorrow)
    .CountAsync();

var earningsToday = await _context.Appointments
    .Where(a => a.AppointmentDate >= today && a.AppointmentDate < tomorrow)
    .SumAsync(a => a.Fee) ?? 0;

    var successRate = 90; // Placeholder
    var previousSuccessRate = 85; 
var successRateChange = successRate - previousSuccessRate;   

    var metrics = new DashboardMetricsDto
{
    TotalPatientsThisMonth = totalPatientsThisMonth,
    TotalPatientsAllTime = totalPatientsAllTime,
    Consultations = consultations,
    Procedures = procedures,
    Payments = payments,
    PreviousPayments = previousPayments,
    SuccessRate = successRate,
    SuccessRateChange = successRateChange,
    AppointmentsToday = appointmentsToday,
    EarningsToday = earningsToday
};


    return Ok(metrics);
}

}
