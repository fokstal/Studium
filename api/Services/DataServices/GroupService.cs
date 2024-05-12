using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class GroupService(AppDbContext db) : DataServiceBase<Group, GroupDTO>(db)
    {
        public override async Task<IEnumerable<Group>> GetListAsync()
        {
            IEnumerable<Group> groupList = await _db.Group.Include(groupDb => groupDb.StudentList).Include(groupdDb => groupdDb.SubjectList).ToArrayAsync();

            return groupList;
        }

        public async Task<Group?> GetAsync(int? id)
        {
            Group? group = await _db.Group
                .Include(groupDb => groupDb.StudentList)
                .Include(groupdDb => groupdDb.SubjectList)
                .FirstOrDefaultAsync(groupdId => groupdId.Id == id);

            return group;
        }

        public async Task<Group?> GetAsync(string name)
        {
            Group? group = await _db.Group
                .Include(groupDb => groupDb.StudentList)
                .Include(groupdDb => groupdDb.SubjectList)
                .FirstOrDefaultAsync(groupdId => groupdId.Name.ToLower() == name.ToLower());

            return group;
        }

        public override async Task UpdateAsync(Group groupToUpdate, GroupDTO groupDTO)
        {
            groupToUpdate.Name = groupDTO.Name;
            groupToUpdate.Description = groupDTO.Name;
            groupToUpdate.Curator = groupDTO.Curator;
            groupToUpdate.AuditoryName = groupDTO.AuditoryName;

            await _db.SaveChangesAsync();
        }
    }
}