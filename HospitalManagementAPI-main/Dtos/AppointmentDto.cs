namespace HospitalManagementAPI.Dtos{
    public class CreateAppointmentDto
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Type { get; set; }
    public decimal? Fee { get; set; }
    public string? Notes { get; set; }
}

}