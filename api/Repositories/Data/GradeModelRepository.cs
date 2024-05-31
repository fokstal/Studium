using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Models.Entities;
using api.Models.DTO;
using api.Helpers.Enums;

namespace api.Repositories.Data
{
    public class GradeModelRepository(AppDbContext db) : DataRepositoryBase<GradeModelEntity, GradeModelDTO>(db)
    {
        public async override Task<IEnumerable<GradeModelEntity>> GetListAsync()
        {
            IEnumerable<GradeModelEntity> gradeList = await
                _db.GradeModel
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.GradeList)
                .ToArrayAsync();

            return gradeList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentIdAsync(Guid id)
        {
            IEnumerable<GradeModelEntity> gradesList = await
                _db.GradeModel
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.GradeList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentList = [];

            foreach (GradeModelEntity grades in gradesList)
            {
                GradeEntity? studentToValueEntity = grades.GradeList.FirstOrDefault(grade => grade.StudentId == id);

                if (studentToValueEntity is not null)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.Type.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentList.Add(new()
                    {
                        StudentId = id,
                        SubjectId = grades.SubjectId,
                        Type = (GradeTypeEnum)gradeTypeEnum,
                        SetDate = grades.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentList;
        }

        public async Task<IEnumerable<GradeModelEntity>> GetListBySubjectIdAsync(int id)
        {
            IEnumerable<GradeModelEntity> gradesList = await
                _db.GradeModel
                .Where(gradesDb => gradesDb.SubjectId == id)
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.GradeList)
                .ToArrayAsync();

            return gradesList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            IEnumerable<GradeModelEntity> gradesList = await
                _db.GradeModel
                .Include(gradeDb => gradeDb.Type)
                .Include(gradeDb => gradeDb.GradeList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentAndSubjectList = [];

            foreach (GradeModelEntity grades in gradesList)
            {
                GradeEntity? studentToValueEntity = grades.GradeList.FirstOrDefault(grade => grade.StudentId == studentId);

                if (studentToValueEntity is not null && grades.SubjectId == subjectId)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.Type.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentAndSubjectList.Add(new()
                    {
                        StudentId = studentId,
                        SubjectId = subjectId,
                        Type = (GradeTypeEnum)gradeTypeEnum,
                        SetDate = grades.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentAndSubjectList;
        }

        public async Task<GradeModelEntity?> GetAsync(DateTime setDate) => await _db.GradeModel.FirstOrDefaultAsync(gradesDb => gradesDb.SetDate == setDate);

        public override GradeModelEntity Create(GradeModelDTO gradesDTO)
        {
            return new()
            {
                SubjectId = gradesDTO.SubjectId,
                SetDate = DateTime.Now.Date,
                Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type),
                GradeList = gradesDTO.StudentToValueList
            };
        }

        public async void UpdateGradeList(GradeModelEntity gradesEntity, HashSet<GradeEntity> gradeList)
        {
            foreach (GradeEntity grade in gradeList)
            {
                gradesEntity.GradeList.Remove(grade);

                gradesEntity.GradeList.Add(grade);
            }

            await _db.SaveChangesAsync();
        }

        public override async Task UpdateAsync(GradeModelEntity gradesToUpdate, GradeModelDTO gradesDTO)
        {
            gradesToUpdate.SubjectId = gradesDTO.SubjectId;
            gradesToUpdate.SetDate = DateTime.Now;
            gradesToUpdate.Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type);
            gradesToUpdate.GradeList = gradesDTO.StudentToValueList;

            await _db.SaveChangesAsync();
        }

        public GradeTypeEntity GetGradeTypeEntitiesByEnum(GradeTypeEnum gradeTypeEnum)
        {
            return _db.GradeType
                    .SingleOrDefault(gradeTypeDb => gradeTypeDb.Name == gradeTypeEnum.ToString())
                    ?? throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");
        }
    }
}