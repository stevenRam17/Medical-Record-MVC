using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IHistorySufferingRepository : IRepository<MedicalHistory_Suffering>
    {
        void Update(MedicalHistory_Suffering obj);

    }
}
