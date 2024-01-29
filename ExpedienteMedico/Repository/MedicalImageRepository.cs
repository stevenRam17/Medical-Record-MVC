using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalImageRepository : Repository<MedicalImage>, IMedicalImageRepository
    {
        private ApplicationDbContext _db;

        public MedicalImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalImage obj)
        {
            _db.MedicalImages.Update(obj);
        }
    }
}
