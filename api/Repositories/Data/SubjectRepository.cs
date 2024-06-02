using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;
using api.Models.Entities;

namespace api.Repositories.Data
{
    public class SubjectRepository(AppDbContext db) : DataRepositoryBase<SubjectEntity, SubjectDTO>(db)
    {
        public async Task<bool> CheckExistsAsync(Guid userId)
            => await _db.Subject.FirstOrDefaultAsync(s => s.TeacherId == userId) is not null;

        public async override Task<IEnumerable<SubjectEntity>> GetListAsync()
        {
            IEnumerable<SubjectEntity> subjectEntityList = await
                _db.Subject
                .Include(s => s.GradeModelEntityList)
                .ToArrayAsync();

            return subjectEntityList;
        }

        public async Task<IEnumerable<SubjectEntity>> GetListAsync(int groupEntityId)
        {
            IEnumerable<SubjectEntity> subjectEntityList = await
                _db.Subject
                .Include(s => s.GradeModelEntityList)
                .ThenInclude(gm => gm.GradeEntityList)
                .Where(s => s.GroupEntityId == groupEntityId)
                .ToArrayAsync();

            return subjectEntityList;
        }

        public async override Task<SubjectEntity?> GetAsync(int subjectEntityId)
        {
            SubjectEntity? subjectEntity = await
                _db.Subject
                .Include(s => s.GradeModelEntityList)
                .FirstOrDefaultAsync(s => s.Id == subjectEntityId);

            return subjectEntity;
        }

        public async Task<SubjectEntity?> GetAsync(string subjectEntityName, Guid? teacherId)
        {
            SubjectEntity? subjectEntity = await
                _db.Subject
                .Include(s => s.GradeModelEntityList)
                .FirstOrDefaultAsync
                (
                    s =>
                        StringComparer.CurrentCultureIgnoreCase.Compare(s.Name, subjectEntityName) == 0 &&
                        s.TeacherId == teacherId
                );

            return subjectEntity;
        }

        public async Task<double> GetAverageGradeAsync(StudentEntity studentEntity)
        {
            IEnumerable<SubjectEntity> subjectEntityList = await
                GetListAsync(groupEntityId: Convert.ToInt32(studentEntity.GroupEntityId));

            return CalculateAverageGrade(subjectEntityList, studentEntity.Id);
        }

        public async Task<double> GetAverageGradeAsync(StudentEntity studentEntity, DateTime startCheck, DateTime endCheck)
        {
            IEnumerable<SubjectEntity> subjectEntityList = await
                GetListAsync(groupEntityId: Convert.ToInt32(studentEntity.GroupEntityId));

            return CalculateAverageGrade(subjectEntityList, studentEntity.Id, startCheck, endCheck);
        }

        public double CalculateAverageGrade(IEnumerable<SubjectEntity> subjectEntityList, Guid studentEntityId)
        {
            double summaryGrades = 0;
            int summarySubjectCount = 0;

            foreach (SubjectEntity subjectEntity in subjectEntityList)
            {
                CalculateAverageGradeSubject
                (
                    ref summaryGrades,
                    ref summarySubjectCount,
                    subjectEntity.GradeModelEntityList,
                    studentEntityId
                );
            }

            return summaryGrades / summarySubjectCount;
        }

        public double CalculateAverageGrade
            (
                IEnumerable<SubjectEntity> subjectEntityList,
                Guid studentEntityId,
                DateTime startCheck,
                DateTime endCheck
            )
        {
            double summaryGrades = 0;
            int summarySubjectCount = 0;

            foreach (SubjectEntity subjectEntity in subjectEntityList)
            {
                List<GradeModelEntity> gradeModelListInInterval =
                    subjectEntity
                    .GradeModelEntityList
                    .Where(gm => gm.SetDate > startCheck && gm.SetDate < endCheck)
                    .ToList();

                CalculateAverageGradeSubject
                (
                    ref summaryGrades,
                    ref summarySubjectCount,
                    gradeModelListInInterval,
                    studentEntityId
                );
            }

            return summaryGrades / summarySubjectCount;
        }

        public void CalculateAverageGradeSubject
            (
                ref double summaryGradeSubject,
                ref int countSubject,
                List<GradeModelEntity> gradeModelListInInterval,
                Guid studentEntityId
            )
        {
            double summGrade = 0;
            int countGrade = 0;

            foreach (GradeModelEntity gradeModelEntity in gradeModelListInInterval)
            {
                GradeEntity? gradeEntity =
                    gradeModelEntity.GradeEntityList
                    .FirstOrDefault(g => g.StudentEntityId == studentEntityId);

                if (gradeEntity is not null)
                {
                    summGrade += gradeEntity.Value;
                    countGrade++;
                }
            }

            if (countGrade != 0)
            {
                summaryGradeSubject += Math.Round(summGrade / countGrade);
                countSubject++;
            }
        }

        public override SubjectEntity Create(SubjectDTO subjectDTO)
        {
            return new()
            {
                Name = subjectDTO.Name,
                Description = subjectDTO.Description,
                TeacherId = subjectDTO.TeacherId,
                GroupEntityId = subjectDTO.GroupEntityId,
            };
        }

        public async override Task UpdateAsync(SubjectEntity subjectEntityToUpdate, SubjectDTO subjectDTO)
        {
            subjectEntityToUpdate.Name = subjectDTO.Name;
            subjectEntityToUpdate.Description = subjectDTO.Description;
            subjectEntityToUpdate.TeacherId = subjectDTO.TeacherId;
            subjectEntityToUpdate.GroupEntityId = subjectDTO.GroupEntityId;

            await _db.SaveChangesAsync();
        }
    }
}