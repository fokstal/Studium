using api.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Services.DataServices
{
    public class SubjectService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        public async Task<bool> CheckExistsAsync(int id) => await _db.Subject.FirstOrDefaultAsync(subjectDb => subjectDb.Id == id) is not null;
    }
}