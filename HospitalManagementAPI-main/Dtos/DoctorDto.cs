namespace HospitalManagementAPI.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
