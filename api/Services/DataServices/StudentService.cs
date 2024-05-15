using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class StudentService(AppDbContext db) : DataServiceBase<StudentEntity, StudentDTO>(db)
    {
        public async Task<StudentEntity?> GetAsync(int personId, int? groupId)
        {
            StudentEntity? student = await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == personId && studentDb.GroupId == groupId);

            return student;
        }

        public async override Task UpdateAsync(StudentEntity studentToUpdate, StudentDTO studentDTO)
        {
            studentToUpdate.PersonId = studentDTO.PersonId;
            studentToUpdate.GroupId = studentDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}