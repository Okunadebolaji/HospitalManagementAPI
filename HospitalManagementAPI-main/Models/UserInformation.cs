using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HospitalManagementAPI.Models;

public class User
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    // 🔹 New field
    public int? CreatedBy { get; set; }

    // 🔗 Navigation
    [ForeignKey("CreatedBy")]
    public User? Creator { get; set; }
    public Role? Role { get; set; }
    // Navigation to Doctor/Nurse
    public Doctor? DoctorProfile { get; set; }
    public Nurse? NurseProfile { get; set; }
}
