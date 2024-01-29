using ExpedienteMedico.Models;

namespace ExpedienteMedico.Repository.IRepository
{
    public interface IMedicalImageRepository : IRepository<MedicalImage>
    {
        void Update(MedicalImage obj);

    }
}
