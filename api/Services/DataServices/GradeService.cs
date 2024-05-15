using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class GradeService(AppDbContext db) : DataServiceBase<GradeEntity, GradeDTO>(db)
    {
        public async Task<IEnumerable<GradeEntity>> GetListByStudentIdAsync(int id)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == id).ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeEntity>> GetListBySubjectIdAsync(int id)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.SubjectId == id).ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeEntity>> GetListByStudentAndSubjectIdAsync(int studentId, int subjectId)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == studentId && grade_db.SubjectId == subjectId).ToListAsync();

            return gradeList;
        }

        public override async Task UpdateAsync(GradeEntity gradeToUpdate, GradeDTO gradeDTO)
        {
            gradeToUpdate.Value = gradeDTO.Value;
            gradeToUpdate.SetDate = DateTime.Now;

            await _db.SaveChangesAsync();
        }
    }
}