namespace HospitalManagementAPI.Dtos
{
    public class AdmissionDto
{
    public int AdmissionId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    
}

}
