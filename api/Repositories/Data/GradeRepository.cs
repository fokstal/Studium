using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Models.Entities;
using api.Models.DTO;
using api.Helpers.Enums;

namespace api.Repositories.Data
{
    public class GradeRepository(AppDbContext db) : DataRepositoryBase<GradesEntity, GradesDTO>(db)
    {
        public async override Task<IEnumerable<GradesEntity>> GetListAsync()
        {
            IEnumerable<GradesEntity> gradeList = await 
                _db.Grades
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.StudentToValueList)
                .ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentIdAsync(Guid id)
        {
            IEnumerable<GradesEntity> gradesList = await 
                _db.Grades
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.StudentToValueList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentList = [];

            foreach (GradesEntity grades in gradesList)
            {
                StudentToValueEntity? studentToValueEntity = grades.StudentToValueList.FirstOrDefault(grade => grade.StudentId == id);

                if (studentToValueEntity is not null)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.Type.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentList.Add(new()
                    {
                        StudentId = id,
                        SubjectId = grades.SubjectId,
                        Type = (GradeTypeEnum) gradeTypeEnum,
                        SetDate = grades.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentList;
        }

        public async Task<IEnumerable<GradesEntity>> GetListBySubjectIdAsync(int id)
        {
            IEnumerable<GradesEntity> gradesList = await 
                _db.Grades
                .Where(gradesDb => gradesDb.SubjectId == id)
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.StudentToValueList)
                .ToArrayAsync();

            return gradesList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            IEnumerable<GradesEntity> gradesList = await 
                _db.Grades
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.StudentToValueList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentAndSubjectList = [];

            foreach (GradesEntity grades in gradesList)
            {
                StudentToValueEntity? studentToValueEntity = grades.StudentToValueList.FirstOrDefault(grade => grade.StudentId == studentId);

                if (studentToValueEntity is not null && grades.SubjectId == subjectId)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.Type.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentAndSubjectList.Add(new()
                    {
                        StudentId = studentId,
                        SubjectId = subjectId,
                        Type = (GradeTypeEnum) gradeTypeEnum,
                        SetDate = grades.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentAndSubjectList;
        }

        public async Task<GradesEntity?> GetAsync(DateTime setDate) => await _db.Grades.FirstOrDefaultAsync(gradesDb => gradesDb.SetDate == setDate);

        public override GradesEntity Create(GradesDTO gradesDTO)
        {
            return new()
            {
                SubjectId = gradesDTO.SubjectId,
                SetDate = DateTime.Now,
                Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type),
                StudentToValueList = gradesDTO.StudentToValueList
            };
        }

        public override async Task UpdateAsync(GradesEntity gradesToUpdate, GradesDTO gradesDTO)
        {
            gradesToUpdate.SubjectId = gradesDTO.SubjectId;
            gradesToUpdate.SetDate = DateTime.Now;
            gradesToUpdate.Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type);
            gradesToUpdate.StudentToValueList = gradesDTO.StudentToValueList;

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