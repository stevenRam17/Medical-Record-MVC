using ExpedienteMedico.Data;
using ExpedienteMedico.Repository.IRepository;
using Microsoft.AspNetCore.Identity;


namespace ExpedienteMedico.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public ISpecialtyRepository Specialty { get; private set; }
        public IPhysicianRepository Physician { get; private set; }

        public IUserRepository User { get; private set; }

        public IPhysicianSpecialty PhysicianSpecialty { get; private set; }

        public ITreatmentRepository Treatment { get; private set; }

        public ISufferingRepository Suffering { get; private set; }

        public IMedicineRepository Medicine { get; private set; }

        public IMedicalHistoryRepository MedicalHistory { get; private set; }

        public IMedicalImageRepository MedicalImage { get; private set; }

        public IMedicalNoteRepository MedicalNote { get; private set; }

        public IHistoryMedicineRepository HistoryMedicine { get; private set; }

        public IHistorySufferingRepository HistorySuffering { get; private set; }

        public IHistoryTreatmentRepository HistoryTreatment { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Specialty = new SpecialtyRepository(_db);
            Physician = new PhysicianRepository(_db);
            User = new UserRepository(_db);
            PhysicianSpecialty = new PhysicianSpecialtyRepository(_db);
            Treatment = new TreatmentRepository(_db);
            Suffering = new SufferingRepository(_db);
            Medicine = new MedicineRepository(_db);
            MedicalHistory = new MedicalHistoryRepository(_db);
            MedicalImage = new MedicalImageRepository(_db);
            MedicalNote = new MedicalNoteRepository(_db);
            HistoryMedicine = new MedicalHistoryMedicineRepository(_db);
            HistorySuffering = new MedicalHistorySufferingRepository(_db);
            HistoryTreatment = new MedicalHistoryTreatmentRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
