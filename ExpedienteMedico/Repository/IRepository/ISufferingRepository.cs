using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface ISufferingRepository : IRepository<Suffering>
    {
        void Update(Suffering obj);

        Suffering GetLast();
    }
}
