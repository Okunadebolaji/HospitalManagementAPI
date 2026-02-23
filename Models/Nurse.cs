namespace HospitalManagementAPI.Models;

public class Nurse
{
    public int NurseId { get; set; }
    public string? Name { get; set; }
    public string? Specialty { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int RolesId { get; set; }
    
   public ICollection<Patient> Patients { get; set; } = new List<Patient>();
        public ICollection<NurseAssignment>? NurseAssignments { get; set; } = new List<NurseAssignment>();
         // Link to User
    public int UserId { get; set; }
    public User? User { get; set; }

}
