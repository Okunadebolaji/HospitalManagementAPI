using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Models;

public class DoctorProfileImage

{
    [Key] 
    public int ImageId { get; set; }
    public int DoctorId { get; set; }
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public byte[] ImageData { get; set; } = null!;
    public bool? IsPrimary { get; set; }
    public DateTime UploadedAt { get; set; }

    public Doctor Doctor { get; set; } = null!;
}
