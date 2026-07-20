namespace HospitalManagementAPI.Dtos
{

   public class AppointmentListDto
{
    public int AppointmentId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Type { get; set; }
    public string? Diagnosis { get; set; }
    public string Status { get; set; } = string.Empty;
    public string DoctorName { get; set; } = string.Empty; 
}


}