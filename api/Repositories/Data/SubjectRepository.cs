using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class SubjectRepository(AppDbContext db) : DataRepositoryBase<SubjectEntity, SubjectDTO>(db)
    {
        public async override Task<IEnumerable<SubjectEntity>> GetListAsync()
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradeList).ToArrayAsync();

            return subjectList;
        }

        public async override Task<SubjectEntity?> GetAsync(int id)
        {
            SubjectEntity? subject = await _db.Subject.Include(subjectDb => subjectDb.GradeList).FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            return subject;
        }

        public async Task<SubjectEntity?> GetAsync(string name, int? teacherId)
        {
            SubjectEntity? subject =
            await _db.Subject
                .Include(subjectDb => subjectDb.GradeList)
                .FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == name.ToLower() &&
                        subjectDb.TeacherId == teacherId
                );

            return subject;
        }

        public async override Task UpdateAsync(SubjectEntity subjectToUpdate, SubjectDTO subjectDTO)
        {
            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Descripton = subjectDTO.Descripton;
            subjectToUpdate.TeacherId = subjectDTO.TeacherId;
            subjectToUpdate.GroupId = subjectDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}