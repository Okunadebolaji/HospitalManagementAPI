namespace HospitalManagementAPI.Dtos
{
    public class CreateAdmissionDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string ReasonForAdmission { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
    }
}
