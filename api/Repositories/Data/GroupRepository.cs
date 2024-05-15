using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class GroupRepository(AppDbContext db) : DataRepositoryBase<GroupEntity, GroupDTO>(db)
    {
        public override async Task<IEnumerable<GroupEntity>> GetListAsync()
        {
            IEnumerable<GroupEntity> groupList = await _db.Group.Include(groupDb => groupDb.StudentList).Include(groupdDb => groupdDb.SubjectList).ToArrayAsync();

            return groupList;
        }

        public async Task<GroupEntity?> GetAsync(int? id)
        {
            GroupEntity? group = await _db.Group
                .Include(groupDb => groupDb.StudentList)
                .Include(groupdDb => groupdDb.SubjectList)
                .FirstOrDefaultAsync(groupdId => groupdId.Id == id);

            return group;
        }

        public async Task<GroupEntity?> GetAsync(string name)
        {
            GroupEntity? group = await _db.Group
                .Include(groupDb => groupDb.StudentList)
                .Include(groupdDb => groupdDb.SubjectList)
                .FirstOrDefaultAsync(groupdId => groupdId.Name.ToLower() == name.ToLower());

            return group;
        }

        public override async Task UpdateAsync(GroupEntity groupToUpdate, GroupDTO groupDTO)
        {
            groupToUpdate.Name = groupDTO.Name;
            groupToUpdate.Description = groupDTO.Name;
            groupToUpdate.Curator = groupDTO.Curator;
            groupToUpdate.AuditoryName = groupDTO.AuditoryName;

            await _db.SaveChangesAsync();
        }
    }
}