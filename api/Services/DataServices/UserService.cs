using api.Data;
using api.Model;
using api.Model.DTO;
using api.Helpers.Enums;
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

        public override async Task AddAsync(User user)
        {
            Models.Role role =
                await _db.Role
                    .SingleOrDefaultAsync(roleDb => roleDb.Id == Convert.ToInt32(Role.User))
                    ?? throw new InvalidOperationException();

            user.RoleList.Add(role);

            await _db.User.AddAsync(user);

            await _db.SaveChangesAsync();
        }

        public override Task UpdateAsync(User valueToUpdate, RegisterUserDTO valueDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<HashSet<Permission>> GetPermissionListAsync(int id)
        {
            List<Models.Role>[] roleList =
                await _db.User
                    .AsNoTracking()
                    .Include(user => user.RoleList)
                    .ThenInclude(role => role.PermissionList)
                    .Where(user => user.Id == id)
                    .Select(user => user.RoleList)
                    .ToArrayAsync();

            return roleList
                .SelectMany(role => role)
                .SelectMany(role => role.PermissionList)
                .Select(permission => (Permission)permission.Id)
                .ToHashSet();
        }
    }
}