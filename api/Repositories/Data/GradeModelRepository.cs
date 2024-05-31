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
        public async Task<GradeModelEntity?> GetAsync(DateTime setDate)
        {
            GradeModelEntity? gradeModel = await
                _db.GradeModel
                .Include(g => g.Type)
                .Include(g => g.GradeList)
                .FirstOrDefaultAsync(gradesDb => gradesDb.SetDate == setDate);

            if (gradeModel is not null)
            {
                foreach (GradeEntity grade in gradeModel.GradeList)
                {
                    grade.GradeModelEntity = null!;
                }
            }

            return gradeModel;
        }

        public async Task<GradeModelEntity?> GetCurrentAsync(DateTime setDate, int subjectId, string typeName)
        {
            GradeModelEntity? gradeModel = await
                _db.GradeModel
                .Include(g => g.Type)
                .Include(g => g.GradeList)
                .FirstOrDefaultAsync
                (
                    gradesDb =>
                        gradesDb.SetDate == setDate &&
                        gradesDb.SubjectId == subjectId &&
                        gradesDb.Type.Name == typeName
                );

            if (gradeModel is not null)
            {
                foreach (GradeEntity grade in gradeModel.GradeList)
                {
                    grade.GradeModelEntity = null!;
                }
            }

            return gradeModel;
        }

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
                SetDate = gradesDTO.SetDate.Date,
                Type = GetGradeTypeEntitiesByEnum(gradesDTO.Type),
                GradeList = gradeList,
            };
        }

        public async Task UpdateGradeList(GradeModelEntity gradesEntity, HashSet<GradeDTO> gradeList)
        {
            HashSet<GradeEntity> gradeEntityList = gradesEntity.GradeList;

            foreach (GradeDTO grade in gradeList)
            {
                gradeEntityList.Add(new()
                {
                    Value = grade.Value,
                    StudentId = grade.StudentId,
                    GradeModelId = gradesEntity.Id
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsOwnSubjectStudent(int subjectId, HashSet<GradeDTO> gradeList)
        {
            SubjectEntity? subject = await _db.Subject.FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subject is not null)
            {
                foreach (GradeDTO grade in gradeList)
                {
                    StudentEntity? student = await _db.Student.FirstOrDefaultAsync(s => s.Id == grade.StudentId);

                    if (student is null) return false;
                    
                    if (student.GroupId != subject.GroupId) return false;
                }
            }

            return true;
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