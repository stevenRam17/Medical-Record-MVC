using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface ITreatmentRepository : IRepository<Treatment>
    {
        void Update(Treatment obj);

        Treatment GetLast();

    }
}
