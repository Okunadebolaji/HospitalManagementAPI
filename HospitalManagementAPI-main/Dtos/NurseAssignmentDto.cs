using HospitalManagementAPI.Models;

// Dtos/NurseAssignmentDto.cs
namespace HospitalManagementAPI.Dtos
{
    public class NurseAssignmentDto
    {
        public int NurseId { get; set; }
        public int AdmissionId { get; set; }
    }
}
