namespace HospitalManagementAPI.Dtos
{

public class StaffCreateDto
{
    public string? Name { get; set; } // for staff
    public string? FirstName { get; set; } // for patient
    public string? LastName { get; set; } // for patient
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int RoleId { get; set; }
    public string? Specialty { get; set; } // for staff
    public string? Bio { get; set; } // for doctor
    public DateTime JoinDate { get; set; } // for staff
    public string? Password { get; set; }
   public int? CreatedBy { get; set; } // optional, allows null

    // Patient-specific
    public string? Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
}


}
