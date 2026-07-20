namespace HospitalManagementAPI.Dtos;

public class DoctorProfileDto
{
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
}
