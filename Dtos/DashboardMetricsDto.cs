namespace HospitalManagementAPI.Dtos
{
    public class DashboardMetricsDto
    {
        public int TotalPatientsThisMonth { get; set; }
        public int TotalPatientsAllTime { get; set; }
        public int Consultations { get; set; }
        public int Procedures { get; set; }
        public decimal Payments { get; set; }
        public decimal PreviousPayments { get; set; }
        public int SuccessRate { get; set; }
        public int AppointmentsToday { get; set; }
        public decimal EarningsToday { get; set; }
        public int SuccessRateChange { get; set; }

    }
}