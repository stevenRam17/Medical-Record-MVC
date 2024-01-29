using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalHistoryRepository : Repository<MedicalHistory>, IMedicalHistoryRepository
    {
        private ApplicationDbContext _db;

        public MedicalHistoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalHistory obj)
        {
            _db.MedicalHistories.Update(obj);
        }
    }
}
