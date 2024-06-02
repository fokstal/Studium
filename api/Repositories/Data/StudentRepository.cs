using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public class StudentRepository(AppDbContext db) : DataRepositoryBase<StudentEntity, StudentDTO>(db)
    {
        public override Task<bool> CheckExistsAsync(int id) => throw new NotImplementedException();

        public async Task<bool> CheckExistsAsync(Guid id) 
            => await _db.Student.FirstOrDefaultAsync(s => s.Id == id) is not null;

        public override Task<StudentEntity?> GetAsync(int id) => throw new NotImplementedException();

        public async Task<StudentEntity?> GetAsync(Guid studentEntityId)
        {
            StudentEntity? studentEntity = await _db.Student.FirstOrDefaultAsync(s => s.Id == studentEntityId);

            return studentEntity;
        }

        public async Task<StudentEntity?> GetAsync(int personEntityId, int? groupEntityId)
        {
            StudentEntity? studentEntity = await 
                _db.Student
                .FirstOrDefaultAsync(s => s.PersonEntityId == personEntityId && s.GroupEntityId == groupEntityId);

            return studentEntity;
        }
        
        public override StudentEntity Create(StudentDTO studentDTO)
        {
            return new()
            {
                Id = studentDTO.Id,
                AddedDate = studentDTO.AddedDate.Date,
                RemovedDate = studentDTO.RemovedDate.Date,
                PersonEntityId = studentDTO.PersonEntityId,
                GroupEntityId = studentDTO.GroupEntityId,
            };
        }

        public async override Task UpdateAsync(StudentEntity studentEntityToUpdate, StudentDTO studentDTO)
        {
            studentEntityToUpdate.Id = studentDTO.Id;
            studentEntityToUpdate.AddedDate = studentDTO.AddedDate.Date;
            studentEntityToUpdate.RemovedDate = studentDTO.RemovedDate.Date;
            studentEntityToUpdate.PersonEntityId = studentDTO.PersonEntityId;
            studentEntityToUpdate.GroupEntityId = studentDTO.GroupEntityId;

            await _db.SaveChangesAsync();
        }
    }
}