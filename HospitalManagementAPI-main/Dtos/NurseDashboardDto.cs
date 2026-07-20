namespace HospitalManagementAPI.Dtos
{
    public class NurseDashboardDto
{
    public List<AssignedPatientDto> Patients { get; set; } = new();
    public List<TaskDto> Tasks { get; set; } = new();
    public List<AlertDto> Alerts { get; set; } = new();
}
}