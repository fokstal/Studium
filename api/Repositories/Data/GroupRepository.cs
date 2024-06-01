using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class GroupRepository(AppDbContext db) : DataRepositoryBase<GroupEntity, GroupDTO>(db)
    {
        public async Task<bool> CheckExistsAsync(Guid userId) => await _db.Group.FirstOrDefaultAsync(valueDb => valueDb.CuratorId == userId) is not null;

        public override async Task<IEnumerable<GroupEntity>> GetListAsync()
        {
            IEnumerable<GroupEntity> groupList = await _db.Group.Include(groupDb => groupDb.StudentEntityList).Include(groupdDb => groupdDb.SubjectEntityList).ToArrayAsync();

            return groupList;
        }

        public async Task<GroupEntity?> GetAsync(int? id)
        {
            GroupEntity? group = await _db.Group
                .Include(groupDb => groupDb.StudentEntityList)
                .Include(groupdDb => groupdDb.SubjectEntityList)
                .FirstOrDefaultAsync(groupdId => groupdId.Id == id);

            return group;
        }

        public async Task<GroupEntity?> GetAsync(string name)
        {
            GroupEntity? group = await _db.Group
                .Include(groupDb => groupDb.StudentEntityList)
                .Include(groupdDb => groupdDb.SubjectEntityList)
                .FirstOrDefaultAsync(groupdId => StringComparer.CurrentCultureIgnoreCase.Compare(groupdId.Name, name) == 0);

            return group;
        }

        public async Task<GroupEntity> GetAsync(SubjectEntity subject)
        {
            GroupEntity? group = await _db.Group
                .Include(groupDb => groupDb.StudentEntityList)
                .Include(groupdDb => groupdDb.SubjectEntityList)
                .FirstOrDefaultAsync(groupdId => groupdId.SubjectEntityList.Contains(subject)) 
                ?? throw new Exception("Subject exists without Group ib Db!");
                
            return group;
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

        public override async Task UpdateAsync(GroupEntity groupToUpdate, GroupDTO groupDTO)
        {
            groupToUpdate.Name = groupDTO.Name;
            groupToUpdate.Description = groupDTO.Name;
            groupToUpdate.CuratorId = groupDTO.CuratorId;
            groupToUpdate.AuditoryName = groupDTO.AuditoryName;

            await _db.SaveChangesAsync();
        }
    }
}