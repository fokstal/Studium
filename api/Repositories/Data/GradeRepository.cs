using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;
using api.Models.Entities;
using api.Helpers.Enums;

namespace api.Repositories.Data
{
    public class GradeRepository(AppDbContext db) : DataRepositoryBase<GradeEntity, GradeDTO>(db)
    {
        public async override Task<IEnumerable<GradeEntity>> GetListAsync()
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Include(gradeDb => gradeDb.Type).ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeEntity>> GetListByStudentIdAsync(Guid id)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == id).Include(gradeDb => gradeDb.Type).ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeEntity>> GetListBySubjectIdAsync(int id)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.SubjectId == id).Include(gradeDb => gradeDb.Type).ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeEntity>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            IEnumerable<GradeEntity> gradeList = await _db.Grade.Where(grade_db => grade_db.StudentId == studentId && grade_db.SubjectId == subjectId).Include(gradeDb => gradeDb.Type).ToListAsync();

            return gradeList;
        }

        public override GradeEntity Create(GradeDTO gradeDTO)
        {
            return new()
            {
                Value = gradeDTO.Value,
                StudentId = gradeDTO.StudentId,
                SubjectId = gradeDTO.SubjectId,
                SetDate = DateTime.Now,
                Type = GetGradeTypeEntitiesByEnum(gradeDTO.Type)
            };
        }

        public override async Task UpdateAsync(GradeEntity gradeToUpdate, GradeDTO gradeDTO)
        {
            gradeToUpdate.Value = gradeDTO.Value;
            gradeToUpdate.SetDate = DateTime.Now;

            await _db.SaveChangesAsync();
        }

        public GradeTypeEntity GetGradeTypeEntitiesByEnum(GradeTypeEnum gradeTypeEnum)
        {
            return _db.GradeTypeEntity
                    .SingleOrDefault(gradeTypeDb => gradeTypeDb.Name == gradeTypeEnum.ToString())
                    ?? throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");
        }
    }
}