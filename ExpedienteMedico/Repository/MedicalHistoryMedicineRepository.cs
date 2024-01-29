using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalHistoryMedicineRepository : Repository<MedicalHistory_Medicine>, IHistoryMedicineRepository
    {
        private ApplicationDbContext _db;

        public MedicalHistoryMedicineRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalHistory_Medicine obj)
        {
            _db.MedicalHistoryMedicines.Update(obj);
        }
    }
}
