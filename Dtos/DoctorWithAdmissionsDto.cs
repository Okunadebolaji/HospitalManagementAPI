namespace HospitalManagementAPI.Dtos
{
    public class DoctorWithAdmissionsDto
    {
        public int DoctorId { get; set; }
         public int UserId { get; set; }     
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public List<AdmissionDto>? Admissions { get; set; }
    }
}
