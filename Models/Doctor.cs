
namespace HospitalManagementAPI.Models;
public class Doctor
{
   public int DoctorId { get; set; }
    public string? Name { get; set; }
    public string? Specialty { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int RoleId { get; set; }
    public int UserId { get; set; }
    public Role Role { get; set; } = null!;

    public string? Bio { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<PatientAdmission> PatientAdmissions { get; set; } = new List<PatientAdmission>();

    public ICollection<DoctorProfileImage>? ProfileImages { get; set; }

     
    
    public User? User { get; set; }

}
