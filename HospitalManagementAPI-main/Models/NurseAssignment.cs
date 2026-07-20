using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementAPI.Models
{
    public class NurseAssignment
    {
        [Key]
        public int NurseAssignmentId { get; set; }

        [ForeignKey(nameof(Nurse))]
        public int NurseId { get; set; }
        public Nurse Nurse { get; set; } = null!;

        [ForeignKey(nameof(Admission))]
        public int AdmissionId { get; set; }
        public PatientAdmission Admission { get; set; } = null!;
    }
}
