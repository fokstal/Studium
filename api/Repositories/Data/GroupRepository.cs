using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class GroupRepository(AppDbContext db) : DataRepositoryBase<GroupEntity, GroupDTO>(db)
    {
        public async Task<bool> CheckExistsAsync(Guid userEntityId) 
            => await _db.Group.FirstOrDefaultAsync(g => g.CuratorId == userEntityId) is not null;

        public override async Task<IEnumerable<GroupEntity>> GetListAsync()
        {
            IEnumerable<GroupEntity> groupEntityList = await 
                _db.Group
                .Include(g => g.StudentEntityList)
                .Include(g => g.SubjectEntityList)
                .ToArrayAsync();

            return groupEntityList;
        }

        public async Task<GroupEntity?> GetAsync(int? groupEntityId)
        {
            GroupEntity? groupEntity = await 
                _db.Group
                .Include(g => g.StudentEntityList)
                .Include(g => g.SubjectEntityList)
                .FirstOrDefaultAsync(g => g.Id == groupEntityId);

            return groupEntity;
        }

        public async Task<GroupEntity?> GetAsync(string groupEntityName)
        {
            GroupEntity? groupEntity = await 
                _db.Group
                .Include(g => g.StudentEntityList)
                .Include(g => g.SubjectEntityList)
                .FirstOrDefaultAsync(g => StringComparer.CurrentCultureIgnoreCase.Compare(g.Name, groupEntityName) == 0);

            return groupEntity;
        }

        public async Task<GroupEntity> GetAsync(SubjectEntity subject)
        {
            GroupEntity? groupEntity = await 
                _db.Group
                .Include(g => g.StudentEntityList)
                .Include(g => g.SubjectEntityList)
                .FirstOrDefaultAsync(g => g.SubjectEntityList.Contains(subject)) 
                ?? throw new Exception("Subject exists without Group ib Db!");
                
            return groupEntity;
        }

        public override GroupEntity Create(GroupDTO groupDTO)
        {
            return new()
            {
                Name = groupDTO.Name,
                Description = groupDTO.Description,
                CuratorId = groupDTO.CuratorId,
                AuditoryName = groupDTO.AuditoryName,
            };
        }

        public override async Task UpdateAsync(GroupEntity groupEntityToUpdate, GroupDTO groupDTO)
        {
            groupEntityToUpdate.Name = groupDTO.Name;
            groupEntityToUpdate.Description = groupDTO.Name;
            groupEntityToUpdate.CuratorId = groupDTO.CuratorId;
            groupEntityToUpdate.AuditoryName = groupDTO.AuditoryName;

            await _db.SaveChangesAsync();
        }
    }
}