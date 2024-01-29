using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpedienteMedico.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Physician> Physicians { get; set; }

        public DbSet<PhysicianSpecialty> PhysicianSpecialties { get; set; }

        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Suffering> Sufferings { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<MedicalHistory> MedicalHistories { get; set; }

        public DbSet<MedicalNote> MedicalNotes { get; set; }

        public DbSet<MedicalImage> MedicalImages { get; set; }

        public DbSet<MedicalHistory_Medicine> MedicalHistoryMedicines { get; set; }

        public DbSet<MedicalHistory_Suffering> MedicalHistorySufferings { get; set; }

        public DbSet<MedicalHistory_Treatment> MedicalHistoryTreatments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //For intermediate tables or models
            modelBuilder.Entity<PhysicianSpecialty>()
                .HasKey(bc => new { bc.PhysicianId, bc.SpecialtyId });

            modelBuilder.Entity<MedicalHistory_Medicine>()
                .HasKey(bc => new { bc.MedicalHistoryId, bc.MedicineId });

            modelBuilder.Entity<MedicalHistory_Suffering>()
                .HasKey(bc => new { bc.MedicalHistoryId, bc.SufferingId });

            modelBuilder.Entity<MedicalHistory_Treatment>()
                .HasKey(bc => new { bc.MedicalHistoryId, bc.TreatmentId });

            modelBuilder.Entity<Physician>().HasOne(d => d.User)
                .WithOne().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MedicalImage>().Property(m => m.PdfUrl).IsRequired(false);

        }
    }

}

