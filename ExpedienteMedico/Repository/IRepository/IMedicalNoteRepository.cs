using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IMedicalNoteRepository : IRepository<MedicalNote>
    {
        void Update(MedicalNote obj);
    }
}
