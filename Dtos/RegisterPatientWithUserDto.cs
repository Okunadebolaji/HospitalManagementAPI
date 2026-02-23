using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Dtos
{
    public class RegisterPatientWithUserDto
    {
        // User fields
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Patient";

        // Patient fields
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public required DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
    
        public required string Email { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
    }
}
