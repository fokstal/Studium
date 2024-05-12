using api.Data;
using api.Model;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class StudentService(AppDbContext db) : DataServiceBase<Student, StudentDTO>(db)
    {
        public async Task<Student?> GetAsync(int personId, int? groupId)
        {
            Student? student = await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.PersonId == personId && studentDb.GroupId == groupId);

            return student;
        }

        public async override Task UpdateAsync(Student studentToUpdate, StudentDTO studentDTO)
        {
            studentToUpdate.PersonId = studentDTO.PersonId;
            studentToUpdate.GroupId = studentDTO.GroupId;

            await _db.SaveChangesAsync();
        }
    }
}