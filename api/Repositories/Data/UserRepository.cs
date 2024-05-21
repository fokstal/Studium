using api.Data;
using api.Models;
using api.Model.DTO;
using api.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using api.Services;

namespace api.Repositories.Data
{
    public class UserRepository(AppDbContext db) : DataRepositoryBase<UserEntity, RegisterUserDTO>(db)
    {
        public override Task<UserEntity?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity?> GetAsync(Guid id)
        {
            UserEntity? user = await _db.User.FirstOrDefaultAsync(userDb => userDb.Id == id);

            return user;
        }

        public async Task<UserEntity?> GetAsync(string login)
        {
            UserEntity? user = await _db.User.FirstOrDefaultAsync(userDb => userDb.Login.ToLower() == login.ToLower());

            return user;
        }

        public override async Task AddAsync(UserEntity user)
        {
            await _db.User.AddAsync(user);

            await _db.SaveChangesAsync();
        }

        public override UserEntity Create(RegisterUserDTO userDTO)
        {
            return new()
            {
                Id = userDTO.Id,
                Login = userDTO.Login,
                FirstName = userDTO.FirstName,
                MiddleName = userDTO.MiddleName,
                LastName = userDTO.LastName,
                PasswordHash = StringHasher.Generate(userDTO.Password),
                DateCreated = DateTime.Now,
                RoleList = userDTO.RoleList
            };
        }

        public override Task UpdateAsync(UserEntity valueToUpdate, RegisterUserDTO valueDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<HashSet<PermissionEnum>> GetPermissionListAsync(Guid id)
        {
            List<RoleEntity>[] roleList = await
                _db.User
                    .AsNoTracking()
                    .Include(user => user.RoleList)
                    .ThenInclude(role => role.PermissionList)
                    .Where(user => user.Id == id)
                    .Select(user => user.RoleList)
                    .ToArrayAsync();

            return roleList
                .SelectMany(role => role)
                .SelectMany(role => role.PermissionList)
                .Select(permission => (PermissionEnum)permission.Id)
                .ToHashSet();
        }

        public async Task<HashSet<RoleEnum>> GetRoleListAsync(Guid id)
        {
            List<RoleEntity>[] roleList = await
                _db.User
                    .AsNoTracking()
                    .Include(user => user.RoleList)
                    .ThenInclude(role => role.PermissionList)
                    .Where(user => user.Id == id)
                    .Select(user => user.RoleList)
                    .ToArrayAsync();

            return roleList
                .SelectMany(role => role)
                .Select(role => (RoleEnum) role.Id)
                .ToHashSet();
        }

        public RoleEntity GetRoleEntityByEnum(RoleEnum roleEnum)
        {
            return _db.Role.SingleOrDefault(roleDb => roleDb.Name == roleEnum.ToString()) ?? throw new Exception("RoleDb and RoleEnum is not Equal!");
        }
    }
}