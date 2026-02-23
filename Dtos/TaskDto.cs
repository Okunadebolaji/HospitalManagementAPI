namespace HospitalManagementAPI.Dtos
{
    public class TaskDto
    {
        public int TaskId { get; set; }

        // Link to admission and patient
        public int AdmissionId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;

        // Task details
        public string Title { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public bool Completed { get; set; }
        public string? Notes { get; set; }
    }
}
