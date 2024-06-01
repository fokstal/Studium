using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;
using api.Models.Entities;

namespace api.Repositories.Data
{
    public class SubjectRepository(AppDbContext db) : DataRepositoryBase<SubjectEntity, SubjectDTO>(db)
    {
        public async Task<bool> CheckExistsAsync(Guid userId) => await _db.Subject.FirstOrDefaultAsync(valueDb => valueDb.TeacherId == userId) is not null;

        public async override Task<IEnumerable<SubjectEntity>> GetListAsync()
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradeModelEntityList).ToArrayAsync();

            return subjectList;
        }

        public async Task<IEnumerable<SubjectEntity>> GetListByGroupAsync(int id)
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Where(subjectDb => subjectDb.GroupEntityId == id).ToArrayAsync();
            IEnumerable<GradeModelEntity> gradesList = await _db.GradeModel.Include(gradesDb => gradesDb.GradeEntityList).ToArrayAsync();

            foreach (SubjectEntity subject in subjectList)
            {
                foreach (GradeModelEntity grades in gradesList)
                {
                    if (subject.Id == grades.SubjectEntityId)
                    {
                        subject.GradeModelEntityList.Add(grades);
                    }
                }
            }

            return subjectList;
        }

        public async override Task<SubjectEntity?> GetAsync(int id)
        {
            SubjectEntity? subject = await _db.Subject.Include(subjectDb => subjectDb.GradeModelEntityList).FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            return subject;
        }

        public async Task<SubjectEntity?> GetAsync(string name, Guid? teacherId)
        {
            SubjectEntity? subject = 
            await _db.Subject
                .Include(subjectDb => subjectDb.GradeModelEntityList)
                .FirstOrDefaultAsync
                (
                    subjectDb =>
                        StringComparer.CurrentCultureIgnoreCase.Compare(subjectDb.Name, name) == 0 &&
                        subjectDb.TeacherId == teacherId
                );

            return subject;
        }

        public async Task<double> GetAverageAsync(StudentEntity student)
        {
            IEnumerable<SubjectEntity> subjectList = await GetListByGroupAsync(Convert.ToInt32(student.GroupEntityId));

            double summaryGrades = 0;

            foreach (SubjectEntity subject in subjectList)
            {
                double summGrades = 0;
                int countGrades = 0;

                foreach (GradeModelEntity gradesEntity in subject.GradeModelEntityList)
                {
                    GradeEntity? grade = gradesEntity.GradeEntityList.FirstOrDefault(grade => grade.StudentEntityId == student.Id);

                    if (grade is not null)
                    {
                        summGrades += grade.Value;
                        countGrades += 1;
                    }
                }

                // if (countGrades >= 3) summaryGrades += Math.Round(summGrades / countGrades);
                summaryGrades += Math.Round(summGrades / countGrades);
            }

            return summaryGrades / subjectList.Count();
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

        public async override Task UpdateAsync(SubjectEntity subjectToUpdate, SubjectDTO subjectDTO)
        {
            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Description = subjectDTO.Description;
            subjectToUpdate.TeacherId = subjectDTO.TeacherId;
            subjectToUpdate.GroupEntityId = subjectDTO.GroupEntityId;

            await _db.SaveChangesAsync();
        }
    }
}