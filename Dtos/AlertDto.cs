namespace HospitalManagementAPI.Dtos
{
    public class AlertDto
    {
        public int? AdmissionId { get; set; }
        public int? PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; // e.g."critical-vitals", "allergy", "overdue-med"(incase i forget) 
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
