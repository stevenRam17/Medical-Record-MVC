using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalHistorySufferingRepository : Repository< MedicalHistory_Suffering>, IHistorySufferingRepository
    {
        private ApplicationDbContext _db;

        public MedicalHistorySufferingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalHistory_Suffering obj)
        {
            _db.MedicalHistorySufferings.Update(obj);
        }
    }
}
