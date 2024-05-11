using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class StudentService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        public async Task<bool> CheckExistsAsync(int id) => await _db.Student.FirstOrDefaultAsync(studentDb => studentDb.Id == id) is not null;
    }
}