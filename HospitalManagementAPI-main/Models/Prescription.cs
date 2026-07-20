namespace HospitalManagementAPI.Models;
public class Prescription
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Specialty { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int DepartmentId { get; set; }
}
