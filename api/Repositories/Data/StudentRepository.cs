using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class StudentRepository(AppDbContext db) : DataRepositoryBase<StudentEntity, StudentDTO>(db)
    {
        public override Task<bool> CheckExistsAsync(int id) => throw new NotImplementedException();

        public async Task<bool> CheckExistsAsync(Guid id) => await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id) is not null;

        public override Task<StudentEntity?> GetAsync(int id) => throw new NotImplementedException();

        public async Task<StudentEntity?> GetAsync(Guid id)
        {
            StudentEntity? student = await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id);

            return student;
        }

        public async Task<StudentEntity?> GetAsync(int personId, int? groupId)
        {
            StudentEntity? student = await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == personId && studentDb.GroupId == groupId);

            return student;
        }
        
        public override StudentEntity Create(StudentDTO studentDTO)
        {
            return new()
            {
                Id = studentDTO.Id,
                AddedDate = studentDTO.AddedDate,
                RemovedDate = studentDTO.RemovedDate,
                PersonId = studentDTO.PersonId,
                GroupId = studentDTO.GroupId,
            };
        }

        public async override Task UpdateAsync(StudentEntity studentToUpdate, StudentDTO studentDTO)
        {
            studentToUpdate.Id = studentDTO.Id;
            studentToUpdate.AddedDate = studentDTO.AddedDate;
            studentToUpdate.RemovedDate = studentDTO.RemovedDate;
            studentToUpdate.PersonId = studentDTO.PersonId;
            studentToUpdate.GroupId = studentDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}