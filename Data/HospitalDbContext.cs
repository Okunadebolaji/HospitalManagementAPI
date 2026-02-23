using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PatientAdmission> PatientAdmissions { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<DoctorProfileImage> DoctorProfileImages { get; set; }
        public DbSet<NurseAssignment> NurseAssignments { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }
        public DbSet<NurseProfileImage> NurseProfileImages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Doctor ↔ Admissions
            modelBuilder.Entity<PatientAdmission>()
                .HasOne(pa => pa.Doctor)
                .WithMany(d => d.PatientAdmissions)
                .HasForeignKey(pa => pa.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admission ↔ NurseAssignments
            modelBuilder.Entity<PatientAdmission>()
                .HasMany(pa => pa.NurseAssignments)
                .WithOne(na => na.Admission)
                .HasForeignKey(na => na.AdmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Nurse ↔ NurseAssignments
            modelBuilder.Entity<Nurse>()
                .HasMany(n => n.NurseAssignments)
                .WithOne(na => na.Nurse)
                .HasForeignKey(na => na.NurseId);

            // Doctor ↔ ProfileImage
            modelBuilder.Entity<DoctorProfileImage>()
                .HasOne(img => img.Doctor)
                .WithMany(d => d.ProfileImages)
                .HasForeignKey(img => img.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nurse ↔ ProfileImage
            modelBuilder.Entity<NurseProfileImage>()
                .HasOne(n => n.Nurse)
                .WithMany()
                .HasForeignKey(n => n.NurseId)
                .OnDelete(DeleteBehavior.Cascade);

            // User ↔ Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Doctor ↔ User
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.DoctorProfile)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nurse ↔ User
            modelBuilder.Entity<Nurse>()
                .HasOne(n => n.User)
                .WithOne(u => u.NurseProfile)
                .HasForeignKey<Nurse>(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Menu ↔ Permissions
            modelBuilder.Entity<MenuPermission>()
                .HasOne(mp => mp.Role)
                .WithMany(r => r.MenuPermissions)
                .HasForeignKey(mp => mp.RoleId);

            modelBuilder.Entity<MenuPermission>()
                .HasOne(mp => mp.Menu)
                .WithMany(m => m.Permissions)
                .HasForeignKey(mp => mp.MenuId);
        }
    }
}
