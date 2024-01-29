using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicineRepository : Repository<Medicine>, IMedicineRepository
    {
        private ApplicationDbContext _db;

        public MedicineRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Medicine obj)
        {
            _db.Medicines.Update(obj);
        }

        public Medicine GetLast()
        {
            return _db.Medicines.OrderByDescending(p => p.Id).FirstOrDefault();
        }
    }
}
