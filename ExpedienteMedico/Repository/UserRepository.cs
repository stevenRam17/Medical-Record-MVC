using ExpedienteMedico.Data;
using ExpedienteMedico.Models;
using ExpedienteMedico.Repository.IRepository;
using ExpedienteMedico.Utility;
using Microsoft.AspNetCore.Identity;

namespace ExpedienteMedico.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            var objFromDB = _db.Users.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDB != null)
            {
                objFromDB.CompleteName = obj.CompleteName;
                objFromDB.UserId = obj.UserId;
                objFromDB.Email = obj.Email;
                objFromDB.PhoneNumber = obj.PhoneNumber;
                objFromDB.LastDateAttended = obj.LastDateAttended;
            }
        }
    }
}
