namespace HospitalManagementAPI.Dtos
{
    public class DoctorCreateDto
    {
        public string Name { get; set; } = null!;
        public string Specialty { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
         public int UserId { get; set; } 
    }
}