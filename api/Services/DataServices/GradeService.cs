using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class GradeService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        public async Task<IEnumerable<Grade>> GetListAsync()
        {
            IEnumerable<Grade> gradeList = await _db.Grade.ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<Grade>> GetListByStudentIdAsync(int id)
        {
            IEnumerable<Grade> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == id).ToListAsync();

            return gradeList;
        }

        public async Task<IEnumerable<Grade>> GetListBySubjectIdAsync(int id)
        {
            IEnumerable<Grade> gradeList = await _db.Grade.Where(grade_db => grade_db.SubjectId == id).ToListAsync();

            return gradeList;
        }

        public async Task<IEnumerable<Grade>> GetListByStudentAndSubjectIdAsync(int studentId, int subjectId)
        {
            IEnumerable<Grade> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == studentId && grade_db.SubjectId == subjectId).ToListAsync();

            return gradeList;
        }

        public async Task<Grade?> GetAsync(int id) => await _db.Grade.FirstOrDefaultAsync(gradeDb => gradeDb.Id == id);

        public async Task AddAsync(GradeDTO gradeDTO)
        {
            await _db.Grade.AddAsync(new()
            {
                Value = gradeDTO.Value,
                StudentId = gradeDTO.StudentId,
                SubjectId = gradeDTO.SubjectId,
                SetDate = DateTime.Now,
            });

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Grade gradeToUpdate, GradeDTO gradeDTO)
        {
            gradeToUpdate.Value = gradeDTO.Value;
            gradeToUpdate.SetDate = DateTime.Now;

            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(Grade gradeToRemove)
        {
            _db.Grade.Remove(gradeToRemove);

            await _db.SaveChangesAsync();
        }

        public async Task<bool> CheckExistsAsync(int id) => await _db.Grade.FirstOrDefaultAsync(gradeDb => gradeDb.Id == id) is not null;
    }
}