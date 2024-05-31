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
            IEnumerable<GradeModelEntity> gradeModelList = await
                _db.GradeModel
                .Include(gradeModelDb => gradeModelDb.Type)
                .Include(gradeModelDb => gradeModelDb.GradeList)
                .ToArrayAsync();

            foreach (GradeModelEntity gradeModel in gradeModelList)
            {
                foreach (GradeEntity grade in gradeModel.GradeList)
                {
                    grade.GradeModelEntity = null!;
                }
            }

            return gradeModelList;
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

        public override Task<GradeModelEntity?> GetAsync(int id) => throw new NotImplementedException();
        public async Task<GradeModelEntity?> GetAsync(Guid id) => await _db.GradeModel.FirstOrDefaultAsync(gradesDb => gradesDb.Id == id);
        public async Task<GradeModelEntity?> GetAsync(DateTime setDate) => await _db.GradeModel.FirstOrDefaultAsync(gradesDb => gradesDb.SetDate == setDate);

        public override GradeModelEntity Create(GradeModelDTO gradesDTO)
        {
            HashSet<GradeEntity> gradeList = [];
            Guid gradeModelId = Guid.NewGuid();

            foreach (GradeDTO grade in gradesDTO.StudentToValueList)
            {
                gradeList.Add(new()
                {
                    Value = grade.Value,
                    StudentId = grade.StudentId,
                    GradeModelId = gradeModelId,
                });
            }

            return new()
            {
                Id = gradeModelId,
                SubjectId = gradesDTO.SubjectId,
                SetDate = DateTime.Now.Date,
                Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type),
                GradeList = gradeList,
            };
        }

        public async void UpdateGradeList(GradeModelEntity gradesEntity, HashSet<GradeDTO> gradeDTOList)
        {
            HashSet<GradeEntity> gradeEntityList = [];

            foreach (GradeDTO grade in gradeDTOList)
            {
                gradeEntityList.Add(new()
                {
                    GradeModelId = gradesEntity.Id,
                    Value = grade.Value,
                    StudentId = grade.StudentId
                });
            }

            foreach (GradeEntity grade in gradeEntityList)
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