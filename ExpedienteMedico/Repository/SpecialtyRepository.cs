using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class SpecialtyRepository : Repository<Specialty>, ISpecialtyRepository
    {
        private ApplicationDbContext _db;

        public SpecialtyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Specialty obj)
        {
            _db.Specialties.Update(obj);
        }
    }
}
