using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class PhysicianRepository : Repository<Physician>, IPhysicianRepository
    {
        private ApplicationDbContext _db;

        public PhysicianRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Physician obj)
        {
            _db.Physicians.Update(obj);
        }

        public Physician GetLast()
        {
            return _db.Physicians.OrderByDescending(p => p.Id).FirstOrDefault();
        }

        public Physician GetByEmail(string email)
        {
            return _db.Physicians.FirstOrDefault(x => x.Email == email);
        }
    }
}
