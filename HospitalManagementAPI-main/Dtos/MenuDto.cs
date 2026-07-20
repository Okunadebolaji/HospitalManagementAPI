namespace HospitalManagementAPI.Dtos
{
    public class MenuDto
    {
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

    }
}