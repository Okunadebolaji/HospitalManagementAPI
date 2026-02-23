namespace HospitalManagementAPI.Models;
public class Menu
{
    public int MenuId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty; // e.g., "/dashboard/doctors"
public ICollection<MenuPermission> Permissions { get; set; } = new List<MenuPermission>();

}
