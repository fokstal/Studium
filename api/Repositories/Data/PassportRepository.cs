using api.Data;
using api.Model.DTO;
using api.Models;

using static api.Repositories.PictureRepository;

namespace api.Repositories.Data
{
    public class PassportRepository(AppDbContext db) : DataRepositoryBase<PassportEntity, PassportDTO>(db)
    {
        public override PassportEntity Create(PassportDTO passportDTO)
        {
            return new()
            {
                ScanFileName = UploadPassportScanAsync(passportDTO.ScanFile).Result,
                PersonEntityId = passportDTO.PersonEntityId,
            };
        }

        public override async Task UpdateAsync(PassportEntity passportToUpdate, PassportDTO passportDTO)
        {
            passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.ScanFile);
            passportToUpdate.PersonEntityId = passportDTO.Id;

            await _db.SaveChangesAsync();
        }
    }
}