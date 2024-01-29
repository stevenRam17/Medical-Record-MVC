using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IHistoryMedicineRepository : IRepository<MedicalHistory_Medicine>
    {
        void Update(MedicalHistory_Medicine obj);

    }
}
