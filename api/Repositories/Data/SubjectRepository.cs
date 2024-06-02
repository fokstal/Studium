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

        public async Task<double> GetAverageAsync(StudentEntity studentEntity)
        {
            IEnumerable<SubjectEntity> subjectEntityList = await 
                GetListAsync(groupEntityId: Convert.ToInt32(studentEntity.GroupEntityId));

            double summaryGrades = 0;

            foreach (SubjectEntity subjectEntity in subjectEntityList)
            {
                double summGrades = 0;
                int countGrades = 0;

                foreach (GradeModelEntity gradeModelEntity in subjectEntity.GradeModelEntityList)
                {
                    GradeEntity? gradeEntity = 
                        gradeModelEntity.GradeEntityList
                        .FirstOrDefault(g => g.StudentEntityId == studentEntity.Id);

                    if (gradeEntity is not null)
                    {
                        summGrades += gradeEntity.Value;
                        countGrades += 1;
                    }
                }

                // if (countGrades >= 3) summaryGrades += Math.Round(summGrades / countGrades);
                summaryGrades += Math.Round(summGrades / countGrades);
            }

            return summaryGrades / subjectEntityList.Count();
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