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
            IEnumerable<GradeModelEntity> gradeModelEntityList = await
                _db.GradeModel
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .ToArrayAsync();

            return gradeModelEntityList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListAsync(Guid studentEntityId)
        {
            IEnumerable<GradeModelEntity> gradeModelEntityList = await
                _db.GradeModel
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentDTOList = [];

            foreach (GradeModelEntity gradeModelEntity in gradeModelEntityList)
            {
                GradeEntity? studentToValueEntity =
                    gradeModelEntity.GradeEntityList.FirstOrDefault(g => g.StudentEntityId == studentEntityId);

                if (studentToValueEntity is not null)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), gradeModelEntity.TypeEntity.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentDTOList.Add(new()
                    {
                        StudentEntityId = studentEntityId,
                        SubjectEntityId = gradeModelEntity.SubjectEntityId,
                        TypeEnum = (GradeTypeEnum)gradeTypeEnum,
                        SetDate = gradeModelEntity.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentDTOList;
        }

        public async Task<IEnumerable<GradeModelEntity>> GetListAsync(int subjectEntityId)
        {
            IEnumerable<GradeModelEntity> gradeModelEntityList = await
                _db.GradeModel
                .Where(gm => gm.SubjectEntityId == subjectEntityId)
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .ToArrayAsync();

            return gradeModelEntityList;
        }

        public async Task<IEnumerable<GradeStudentDTO>> GetListAsync(Guid studentEntityId, int subjectEntityId)
        {
            IEnumerable<GradeModelEntity> gradeModelEntityList = await
                _db.GradeModel
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .ToArrayAsync();

            List<GradeStudentDTO> gradeStudentDTOList = [];

            foreach (GradeModelEntity gradeModelEntity in gradeModelEntityList)
            {
                GradeEntity? studentToValueEntity =
                    gradeModelEntity.GradeEntityList.FirstOrDefault(g => g.StudentEntityId == studentEntityId);

                if (studentToValueEntity is not null && gradeModelEntity.SubjectEntityId == subjectEntityId)
                {
                    Enum.TryParse(typeof(GradeTypeEnum), gradeModelEntity.TypeEntity.Name, out object? gradeTypeEnum);

                    if (gradeTypeEnum is null) throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");

                    gradeStudentDTOList.Add(new()
                    {
                        StudentEntityId = studentEntityId,
                        SubjectEntityId = subjectEntityId,
                        TypeEnum = (GradeTypeEnum)gradeTypeEnum,
                        SetDate = gradeModelEntity.SetDate,
                        Value = studentToValueEntity.Value,
                    });
                }
            }

            return gradeStudentDTOList;
        }

        public override Task<GradeModelEntity?> GetAsync(int id) => throw new NotImplementedException();

        public async Task<GradeModelEntity?> GetAsync(Guid studentEntityId)
            => await _db.GradeModel.FirstOrDefaultAsync(gm => gm.Id == studentEntityId);

        public async Task<GradeModelEntity?> GetAsync(DateTime setDate)
        {
            GradeModelEntity? gradeModelEntity = await
                _db.GradeModel
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .FirstOrDefaultAsync(gm => gm.SetDate == setDate);

            return gradeModelEntity;
        }

        public async Task<GradeModelEntity?> GetAsync(DateTime setDate, int subjectEntityId, string typeName)
        {
            GradeModelEntity? gradeModelEntity = await
                _db.GradeModel
                .Include(gm => gm.TypeEntity)
                .Include(gm => gm.GradeEntityList)
                .FirstOrDefaultAsync
                (
                    gm =>
                        gm.SetDate == setDate &&
                        gm.SubjectEntityId == subjectEntityId &&
                        gm.TypeEntity.Name == typeName
                );

            return gradeModelEntity;
        }

        public override GradeModelEntity Create(GradeModelDTO gradeModelDTO)
        {
            List<GradeEntity> gradeEntityList = [];
            Guid gradeModelId = Guid.NewGuid();

            foreach (GradeDTO gradeDTO in gradeModelDTO.GradeDTOList)
            {
                gradeEntityList.Add(new()
                {
                    Value = gradeDTO.Value,
                    StudentEntityId = gradeDTO.StudentEntityId,
                    GradeModelEntityId = gradeModelId,
                });
            }

            return new()
            {
                Id = gradeModelId,
                SubjectEntityId = gradeModelDTO.SubjectEntityId,
                SetDate = gradeModelDTO.SetDate.Date,
                TypeEntity = GetGradeTypeEntitiesByEnum(gradeModelDTO.TypeEnum),
                GradeEntityList = gradeEntityList,
            };
        }

        public async Task UpdateGradeListAsync(GradeModelEntity gradeModelEntity, GradeModelDTO gradeModelDTO)
        {
            List<GradeEntity> gradeEntityList = await
                _db.Grade
                .Where(g => g.GradeModelEntityId == gradeModelEntity.Id)
                .ToListAsync();

            foreach (GradeDTO gradeDTO in gradeModelDTO.GradeDTOList)
            {
                GradeEntity? existedGradeEntity =
                    gradeEntityList.FirstOrDefault(g => g.StudentEntityId == gradeDTO.StudentEntityId);

                if (existedGradeEntity is not null) existedGradeEntity.Value = gradeDTO.Value;

                if (existedGradeEntity is null)
                {
                    gradeEntityList.Add(new()
                    {
                        StudentEntityId = gradeDTO.StudentEntityId,
                        Value = gradeDTO.Value,
                        GradeModelEntityId = gradeModelEntity.Id
                    });
                }
            }

            gradeModelEntity.GradeEntityList = gradeEntityList;

            await _db.SaveChangesAsync();
        }

        public async Task<bool> IsOwnSubjectStudent(int subjectId, List<GradeDTO> gradeDTOList)
        {
            SubjectEntity? subjectEntity = await _db.Subject.FirstOrDefaultAsync(s => s.Id == subjectId);

            if (subjectEntity is not null)
            {
                foreach (GradeDTO gradeDTO in gradeDTOList)
                {
                    StudentEntity? studentEntity = await 
                        _db.Student.FirstOrDefaultAsync(s => s.Id == gradeDTO.StudentEntityId);

                    if (studentEntity is null) return false;

                    if (studentEntity.GroupEntityId != subjectEntity.GroupEntityId) return false;
                }
            }

            return true;
        }

        public override async Task UpdateAsync(GradeModelEntity gradeModelEntityToUpdate, GradeModelDTO gradeModelDTO)
        {
            gradeModelEntityToUpdate.SubjectEntityId = gradeModelDTO.SubjectEntityId;
            gradeModelEntityToUpdate.SetDate = gradeModelDTO.SetDate.Date;
            gradeModelEntityToUpdate.TypeEntity = GetGradeTypeEntitiesByEnum(gradeModelDTO.TypeEnum);

            await _db.SaveChangesAsync();
        }

        public GradeTypeEntity GetGradeTypeEntitiesByEnum(GradeTypeEnum gradeTypeEnum)
        {
            return _db.GradeType
                    .SingleOrDefault(gt => gt.Name == gradeTypeEnum.ToString())
                    ?? throw new Exception("GradeTypeDb and GradeTypeEnum is not Equal!");
        }
    }
}