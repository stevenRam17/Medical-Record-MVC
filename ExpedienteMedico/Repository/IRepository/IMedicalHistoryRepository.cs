using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IMedicalHistoryRepository : IRepository<MedicalHistory>
    {
        void Update(MedicalHistory obj);

    }
}
