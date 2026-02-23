using System.ComponentModel.DataAnnotations;
namespace HospitalManagementAPI.Models
{
    public class NurseProfileImage
    {
        [Key]
        public int ImageId { get; set; }
        public int NurseId { get; set; }  
        public Nurse Nurse { get; set; } = null!;

        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public bool IsPrimary { get; set; } = true;
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}


