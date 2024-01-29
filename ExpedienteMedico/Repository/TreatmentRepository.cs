using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class TreatmentRepository : Repository<Treatment>, ITreatmentRepository
    {
        private ApplicationDbContext _db;

        public TreatmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Treatment obj)
        {
            _db.Treatments.Update(obj);
        }

        public Treatment GetLast()
        {
            return _db.Treatments.OrderByDescending(p => p.Id).FirstOrDefault();
        }
    }
}
