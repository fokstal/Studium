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
        public async override Task<IEnumerable<UserEntity>> GetListAsync()
        {
            IEnumerable<UserEntity> userEntityList = await
                _db.User
                .Select(userDb => new UserEntity()
                {
                    Id = userDb.Id,
                    Login = userDb.Login,
                    FirstName = userDb.FirstName,
                    MiddleName = userDb.MiddleName,
                    LastName = userDb.LastName,
                    PasswordHash = userDb.PasswordHash,
                    DateCreated = userDb.DateCreated,
                    RoleEntityList =
                        userDb.RoleEntityList
                        .Select(roleDb => new RoleEntity { Id = roleDb.Id, Name = roleDb.Name })
                        .ToList(),
                })
                .ToListAsync();

            return userEntityList;
        }

        public override Task<UserEntity?> GetAsync(int id) => throw new NotImplementedException();

        public async Task<UserEntity?> GetAsync(Guid userEntityId)
        {
            UserEntity? userEntity = await
                _db.User
                .Include(u => u.RoleEntityList)
                .FirstOrDefaultAsync(u => u.Id == userEntityId);

            if (userEntity is not null)
            {
                userEntity.RoleEntityList =
                    userEntity.RoleEntityList
                    .Select(r => new RoleEntity { Id = r.Id, Name = r.Name })
                    .ToList();
            }

            return userEntity;
        }

        public async Task<UserEntity?> GetNoTrackingAsync(Guid userEntityId)
        {
            UserEntity? userEntity = await
                _db.User
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userEntityId);

            return userEntity;
        }

        public async Task<UserEntity?> GetAsync(string login)
        {
            UserEntity? userEntity = await
                _db.User
                .FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());

            return userEntity;
        }

        public override UserEntity Create(RegisterUserDTO registerUserDTO)
        {
            return new()
            {
                Id = registerUserDTO.Id,
                Login = registerUserDTO.Login,
                FirstName = registerUserDTO.FirstName,
                MiddleName = registerUserDTO.MiddleName,
                LastName = registerUserDTO.LastName,
                PasswordHash = StringHasher.Generate(registerUserDTO.Password),
                DateCreated = DateTime.Now,
                RoleEntityList = GetRolesEntityByEnum(registerUserDTO.RoleEnumList),
            };
        }

        public override Task UpdateAsync(UserEntity valueToUpdate, RegisterUserDTO valueDTO) => throw new NotImplementedException();

        public async Task<HashSet<PermissionEnum>> GetPermissionListAsync(Guid id)
        {
            List<RoleEntity>[] roleEntityList = await
                _db.User
                    .AsNoTracking()
                    .Include(u => u.RoleEntityList)
                    .ThenInclude(r => r.PermissionEntityList)
                    .Where(u => u.Id == id)
                    .Select(u => u.RoleEntityList)
                    .ToArrayAsync();

            return roleEntityList
                .SelectMany(r => r)
                .SelectMany(r => r.PermissionEntityList)
                .Select(p => (PermissionEnum)p.Id)
                .ToHashSet();
        }

        public async Task<HashSet<RoleEnum>> GetRoleListAsync(Guid userId)
        {
            List<RoleEntity>[] roleEntityList = await
                _db.User
                .AsNoTracking()
                .Include(u => u.RoleEntityList)
                .ThenInclude(r => r.PermissionEntityList)
                .Where(u => u.Id == userId)
                .Select(u => u.RoleEntityList)
                .ToArrayAsync();

            return roleEntityList
                .SelectMany(r => r)
                .Select(r => (RoleEnum)r.Id)
                .ToHashSet();
        }

        public List<RoleEntity> GetRolesEntityByEnum(RoleEnum[] roleEnumList)
        {
            List<RoleEntity> roleEntityList = [];

            foreach (RoleEnum roleEnum in roleEnumList)
            {
                roleEntityList.Add
                (
                    _db.Role
                    .SingleOrDefault(r => r.Name == roleEnum.ToString())
                    ?? throw new Exception("RoleDb and RoleEnum is not Equal!")
                );
            }

            return roleEntityList;
        }
    }
}