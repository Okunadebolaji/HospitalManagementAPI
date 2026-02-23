using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Models;
public class PatientAdmission
{
    [Key]
    public int AdmissionId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime? AdmissionDate { get; set; }
    public DateTime? DischargeDate { get; set; }
    public string ReasonForAdmission { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;

    // One admission can have many nurse assignments
    public ICollection<NurseAssignment> NurseAssignments { get; set; } = new List<NurseAssignment>();
}
