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
                .Include(gradeModelDb => gradeModelDb.TypeEntity)
                .Include(gradeModelDb => gradeModelDb.GradeEntityList)
                .ToArrayAsync();

            return gradeModelList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentIdAsync(Guid id)
        {
            IEnumerable<GradeModelEntity> gradesList = await
                _db.GradeModel
                .Include(gradeDb => gradeDb.TypeEntity)
                .Include(gradeDb => gradeDb.GradeEntityList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentList = [];

            foreach (GradeModelEntity grades in gradesList)
            {
                GradeEntity? studentToValueEntity = grades.GradeEntityList.FirstOrDefault(grade => grade.StudentEntityId == id);

                if (studentToValueEntity is not null)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.TypeEntity.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentList.Add(new()
                    {
                        StudentEntityId = id,
                        SubjectEntityId = grades.SubjectEntityId,
                        TypeEnum = (GradeTypeEnum)gradeTypeEnum,
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
                .Where(gradesDb => gradesDb.SubjectEntityId == id)
                .Include(gradeDb => gradeDb.TypeEntity)
                .Include(gradeDb => gradeDb.GradeEntityList)
                .ToArrayAsync();

            return gradesList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListByStudentAndSubjectIdAsync(Guid studentId, int subjectId)
        {
            IEnumerable<GradeModelEntity> gradesList = await
                _db.GradeModel
                .Include(gradeDb => gradeDb.TypeEntity)
                .Include(gradeDb => gradeDb.GradeEntityList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentAndSubjectList = [];

            foreach (GradeModelEntity grades in gradesList)
            {
                GradeEntity? studentToValueEntity = grades.GradeEntityList.FirstOrDefault(grade => grade.StudentEntityId == studentId);

                if (studentToValueEntity is not null && grades.SubjectEntityId == subjectId)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), grades.TypeEntity.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentAndSubjectList.Add(new()
                    {
                        StudentEntityId = studentId,
                        SubjectEntityId = subjectId,
                        TypeEnum = (GradeTypeEnum)gradeTypeEnum,
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
                .Include(g => g.TypeEntity)
                .Include(g => g.GradeEntityList)
                .FirstOrDefaultAsync(gradesDb => gradesDb.SetDate == setDate);

            return gradeModel;
        }

        public async Task<GradeModelEntity?> GetCurrentAsync(DateTime setDate, int subjectId, string typeName)
        {
            GradeModelEntity? gradeModel = await
                _db.GradeModel
                .Include(g => g.TypeEntity)
                .Include(g => g.GradeEntityList)
                .FirstOrDefaultAsync
                (
                    gradesDb =>
                        gradesDb.SetDate == setDate &&
                        gradesDb.SubjectEntityId == subjectId &&
                        gradesDb.TypeEntity.Name == typeName
                );

            return gradeModel;
        }

        public override GradeModelEntity Create(GradeModelDTO gradesDTO)
        {
            List<GradeEntity> gradeList = [];
            Guid gradeModelId = Guid.NewGuid();

            foreach (GradeDTO grade in gradesDTO.GradeDTOList)
            {
                gradeList.Add(new()
                {
                    Value = grade.Value,
                    StudentEntityId = grade.StudentEntityId,
                    GradeModelEntityId = gradeModelId,
                });
            }

            return new()
            {
                Id = gradeModelId,
                SubjectEntityId = gradesDTO.SubjectEntityId,
                SetDate = gradesDTO.SetDate.Date,
                TypeEntity = GetGradeTypeEntitiesByEnum(gradesDTO.TypeEnum),
                GradeEntityList = gradeList,
            };
        }

        public async Task UpdateGradeListAsync(GradeModelEntity gradeModelEntity, GradeModelDTO gradeModelDTO)
        {
            List<GradeEntity> gradeList = await _db.Grade.Where(g => g.GradeModelEntityId == gradeModelEntity.Id).ToListAsync();

            foreach (var gradeDTO in gradeModelDTO.GradeDTOList)
            {
                GradeEntity? existedGradeEntity = gradeList.FirstOrDefault(g => g.StudentEntityId == gradeDTO.StudentEntityId);

                if (existedGradeEntity is not null)
                {
                    existedGradeEntity.Value = gradeDTO.Value;
                }

                if (existedGradeEntity is null)
                {
                    gradeList.Add(new()
                    {
                        StudentEntityId = gradeDTO.StudentEntityId,
                        Value = gradeDTO.Value,
                        GradeModelEntityId = gradeModelEntity.Id
                    });
                }
            }

            gradeModelEntity.GradeEntityList = gradeList;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsOwnSubjectStudent(int subjectId, List<GradeDTO> gradeList)
        {
            SubjectEntity? subject = await _db.Subject.FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subject is not null)
            {
                foreach (GradeDTO grade in gradeList)
                {
                    StudentEntity? student = await _db.Student.FirstOrDefaultAsync(s => s.Id == grade.StudentEntityId);

                    if (student is null) return false;

                    if (student.GroupEntityId != subject.GroupEntityId) return false;
                }
            }

            return true;
        }

        public override async Task UpdateAsync(GradeModelEntity gradesToUpdate, GradeModelDTO gradesDTO)
        {
            gradesToUpdate.SubjectEntityId = gradesDTO.SubjectEntityId;
            gradesToUpdate.SetDate = DateTime.Now;
            gradesToUpdate.TypeEntity = GetGradeTypeEntitiesByEnum(gradesDTO.TypeEnum);

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