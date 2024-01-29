using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IPhysicianRepository : IRepository<Physician>
    {
        void Update(Physician obj);

        Physician GetLast();

        Physician GetByEmail(string email);
    }
}
