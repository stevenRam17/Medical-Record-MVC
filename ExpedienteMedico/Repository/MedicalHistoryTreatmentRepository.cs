using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Models.IntermediateTables;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalHistoryTreatmentRepository : Repository<MedicalHistory_Treatment>, IHistoryTreatmentRepository
    {
        private ApplicationDbContext _db;

        public MedicalHistoryTreatmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalHistory_Treatment obj)
        {
            _db.MedicalHistoryTreatments.Update(obj);
        }
    }
}
