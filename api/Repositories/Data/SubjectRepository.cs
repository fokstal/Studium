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
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Include(subjectDb => subjectDb.GradesList).ToArrayAsync();

            return subjectList;
        }

        public async Task<IEnumerable<SubjectEntity>> GetListByGroupAsync(int id)
        {
            IEnumerable<SubjectEntity> subjectList = await _db.Subject.Where(subjectDb => subjectDb.GroupId == id).ToArrayAsync();
            IEnumerable<GradesEntity> gradesList = await _db.Grades.Include(gradesDb => gradesDb.StudentToValueList).ToArrayAsync();

            foreach (SubjectEntity subject in subjectList)
            {
                foreach (GradesEntity grades in gradesList)
                {
                    if (subject.Id == grades.SubjectId)
                    {
                        subject.GradesList.Add(grades);
                    }
                }
            }

            return subjectList;
        }

        public async override Task<SubjectEntity?> GetAsync(int id)
        {
            SubjectEntity? subject = await _db.Subject.Include(subjectDb => subjectDb.GradesList).FirstOrDefaultAsync(subjectDb => subjectDb.Id == id);

            return subject;
        }

        public async Task<SubjectEntity?> GetAsync(string name, Guid? teacherId)
        {
            SubjectEntity? subject = 
            await _db.Subject
                .Include(subjectDb => subjectDb.GradesList)
                .FirstOrDefaultAsync
                (
                    subjectDb =>
                        subjectDb.Name.ToLower() == name.ToLower() &&
                        subjectDb.TeacherId == teacherId
                );

            return subject;
        }

        public async Task<double> GetAverageAsync(StudentEntity student)
        {
            IEnumerable<SubjectEntity> subjectList = await GetListByGroupAsync(Convert.ToInt32(student.GroupId));

            double summaryGrades = 0;

            foreach (SubjectEntity subject in subjectList)
            {
                double summGrades = 0;
                int countGrades = 0;

                foreach (GradesEntity gradesEntity in subject.GradesList)
                {
                    StudentToValueEntity? grade = gradesEntity.StudentToValueList.FirstOrDefault(grade => grade.StudentId == student.Id);

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
                GroupId = subjectDTO.GroupId,
            };
        }

        public async override Task UpdateAsync(SubjectEntity subjectToUpdate, SubjectDTO subjectDTO)
        {
            subjectToUpdate.Name = subjectDTO.Name;
            subjectToUpdate.Description = subjectDTO.Description;
            subjectToUpdate.TeacherId = subjectDTO.TeacherId;
            subjectToUpdate.GroupId = subjectDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}