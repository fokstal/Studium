using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class SubjectRepository(AppDbContext db) : DataRepositoryBase<SubjectEntity, SubjectDTO>(db)
    {
        public async Task<bool> CheckExistsAsync(Guid userId) => await _db.Subject.FirstOrDefaultAsync(valueDb => valueDb.TeacherId == userId) is not null;

        public async override Task<IEnumerable<SubjectEntity>> GetListAsync()
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradesList).ToArrayAsync();

            return subjectList;
        }

        public async Task<IEnumerable<SubjectEntity>> GetListByGroupAsync(int id)
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Where(subjectDb => subjectDb.GroupId == id).Include(subjectDb => subjectDb.GradesList).ToArrayAsync();

            return subjectList;
        }

        public async override Task<SubjectEntity?> GetAsync(int id)
        {
            SubjectEntity? subject = await _db.Subject.Include(subjectDb => subjectDb.GradesList).FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            return subject;
        }

        public async Task<SubjectEntity?> GetAsync(string name, Guid? teacherId)
        {
            SubjectEntity? subject =
            await _db.Subject
                .Include(subjectDb => subjectDb.GradesList)
                .FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == name.ToLower() &&
                        subjectDb.TeacherId == teacherId
                );

            return subject;
        }

        public override SubjectEntity Create(SubjectDTO subjectDTO)
        {
            return new()
            {
                Name = subjectDTO.Name,
                Description = subjectDTO.Description,
                TeacherId = subjectDTO.TeacherId,
                GroupId = subjectDTO.GroupId,
            };
        }

        public async override Task UpdateAsync(SubjectEntity subjectToUpdate, SubjectDTO subjectDTO)
        {
            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Description = subjectDTO.Description;
            subjectToUpdate.TeacherId = subjectDTO.TeacherId;
            subjectToUpdate.GroupId = subjectDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}