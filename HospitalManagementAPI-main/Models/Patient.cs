namespace HospitalManagementAPI.Models;

public class Patient
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }
    public string? Allergies { get; set; }
    public DateTime CreatedAt { get; set; }


   public  int UserId { get; set; } 
public User? User { get; set; } 

public int? NurseId { get; set;}
public Nurse? Nurse { get; set;}

public ICollection<PatientAdmission> PatientAdmissions { get; set; } = new List<PatientAdmission>();



}
