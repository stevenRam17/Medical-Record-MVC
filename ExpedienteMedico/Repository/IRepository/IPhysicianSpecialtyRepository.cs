using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IPhysicianSpecialty : IRepository<PhysicianSpecialty>
    {
        void Update(PhysicianSpecialty obj);
        
    }
}
