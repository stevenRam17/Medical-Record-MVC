using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IMedicineRepository : IRepository<Medicine>
    {
        void Update(Medicine obj);

        Medicine GetLast();
    }
}
