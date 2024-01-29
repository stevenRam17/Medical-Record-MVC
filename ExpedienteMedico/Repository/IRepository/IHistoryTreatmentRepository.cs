using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IHistoryTreatmentRepository : IRepository<MedicalHistory_Treatment>
    {
        void Update(MedicalHistory_Treatment obj);

    }
}
