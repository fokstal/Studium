using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class SubjectService(AppDbContext db) : DataServiceBase<Subject, SubjectDTO>(db)
    {
        public async override Task<IEnumerable<Subject>> GetListAsync()
        {
            IEnumerable<Subject> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradeList).ToArrayAsync();

            return subjectList;
        }

        public async override Task<Subject?> GetAsync(int id)
        {
            Subject? subject = await _db.Subject.Include(subjectDb => subjectDb.GradeList).FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            return subject;
        }

        public async Task<Subject?> GetAsync(string name, string teacherName)
        {
            Subject? subject =
            await _db.Subject
                .Include(subjectDb => subjectDb.GradeList)
                .FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == name.ToLower() &&
                        subjectDb.TeacherName.ToLower() == teacherName.ToLower()
                );

            return subject;
        }

        public async override Task UpdateAsync(Subject subjectToUpdate, SubjectDTO subjectDTO)
        {
            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Descripton = subjectDTO.Descripton;
            subjectToUpdate.TeacherName = subjectDTO.TeacherName;
            subjectToUpdate.GroupId = subjectDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}