using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface ISpecialtyRepository : IRepository<Specialty>
    {
        void Update(Specialty obj);

    }
}
