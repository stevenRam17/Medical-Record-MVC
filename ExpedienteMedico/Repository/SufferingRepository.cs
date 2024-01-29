using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class SufferingRepository : Repository<Suffering>, ISufferingRepository
    {
        private ApplicationDbContext _db;

        public SufferingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Suffering obj)
        {
            _db.Sufferings.Update(obj);
        }

        public Suffering GetLast()
        {
            return _db.Sufferings.OrderByDescending(p => p.Id).FirstOrDefault();
        }
    }
}
