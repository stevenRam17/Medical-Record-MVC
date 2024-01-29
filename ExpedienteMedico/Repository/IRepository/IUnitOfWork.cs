namespace ExpedienteMedico.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ISpecialtyRepository Specialty { get; }
        IPhysicianRepository Physician { get; }
        IUserRepository User { get; }

        IPhysicianSpecialty PhysicianSpecialty { get; }

        ITreatmentRepository Treatment { get; }

        ISufferingRepository Suffering { get; }

        IMedicineRepository Medicine { get; }

        IMedicalHistoryRepository MedicalHistory { get; }

        IMedicalNoteRepository MedicalNote { get; }

        IMedicalImageRepository MedicalImage { get; }

        IHistoryMedicineRepository HistoryMedicine { get; }

        IHistorySufferingRepository HistorySuffering { get; }

        IHistoryTreatmentRepository HistoryTreatment { get; }
        void Save();
    }
}
