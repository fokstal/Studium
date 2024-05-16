using api.Data;
using api.Model.DTO;
using api.Models;
using static api.Repositories.PictureRepository;

namespace api.Repositories.Data
{
    public class PassportRepository(AppDbContext db) : DataRepositoryBase<PassportEntity, PassportDTO>(db)
    {
        public override async Task UpdateAsync(PassportEntity passportToUpdate, PassportDTO passportDTO)
        {
            passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.Scan);
            passportToUpdate.PersonId = passportDTO.Id;

            await _db.SaveChangesAsync();
        }
    }
}