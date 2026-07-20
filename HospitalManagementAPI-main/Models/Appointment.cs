namespace HospitalManagementAPI.Models;
public class Appointment
{
    public int AppointmentId { get; set; }
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Type { get; set; }
    public decimal? Fee { get; set; }
    public string? Notes { get; set; }

    //  Navigation to Doctors
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }


   public required string Status { get; set; } = "Pending";
}
