namespace HospitalManagementAPI.Models;
public class MenuPermission
{
    public int MenuPermissionId { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int MenuId { get; set; }
    public Menu? Menu { get; set; }

    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
      public bool CanDelete { get; set; } 
}
