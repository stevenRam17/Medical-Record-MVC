using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class PhysicianSpecialtyRepository : Repository<PhysicianSpecialty>, IPhysicianSpecialty
    {
        private ApplicationDbContext _db;

        public PhysicianSpecialtyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PhysicianSpecialty obj)
        {
            _db.PhysicianSpecialties.Update(obj);
        }
    }
}
