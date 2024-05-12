using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class UserService(AppDbContext db) : DataServiceBase<User, RegisterUserDTO>(db)
    {
        public async Task<User?> GetAsync(string login)
        {
            User? user = await _db.User.FirstOrDefaultAsync(userDb => userDb.Login.ToLower() == login.ToLower());

            return user;
        }

        public override Task UpdateAsync(User valueToUpdate, RegisterUserDTO valueDTO)
        {
            throw new NotImplementedException();
        }
    }
}