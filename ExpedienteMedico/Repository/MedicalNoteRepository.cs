using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;

namespace ExpedienteMedico.Repository
{
    public class MedicalNoteRepository : Repository<MedicalNote>, IMedicalNoteRepository
    {
        private ApplicationDbContext _db;

        public MedicalNoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MedicalNote obj)
        {
            _db.MedicalNotes.Update(obj);
        }
    }
}
