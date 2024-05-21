using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class StudentRepository(AppDbContext db) : DataRepositoryBase<StudentEntity, StudentDTO>(db)
    {
        public async Task<StudentEntity?> GetAsync(int personId, int? groupId)
        {
            StudentEntity? student = await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == personId && studentDb.GroupId == groupId);

            return student;
        }
        
        public override StudentEntity Create(StudentDTO studentDTO)
        {
            return new()
            {
                AddedDate = studentDTO.AddedDate,
                RemovedDate = studentDTO.RemovedDate,
                PersonId = studentDTO.PersonId,
                GroupId = studentDTO.GroupId,
            };
        }

        public async override Task UpdateAsync(StudentEntity studentToUpdate, StudentDTO studentDTO)
        {
            studentToUpdate.AddedDate = studentDTO.AddedDate;
            studentToUpdate.RemovedDate = studentDTO.RemovedDate;
            studentToUpdate.PersonId = studentDTO.PersonId;
            studentToUpdate.GroupId = studentDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}