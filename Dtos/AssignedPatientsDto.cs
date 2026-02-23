namespace HospitalManagementAPI.Dtos
{
    public class AssignedPatientDto
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string ConditionSummary { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public DateTime? ExpectedDischargeDate { get; set; }
}
}