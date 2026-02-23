namespace HospitalManagementAPI.Dtos{
   public class CreatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } // Optional
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? BloodType { get; set; }
    public string? Allergies { get; set; }

    public int UserId { get; set; } // Required to link patient to user
}


}