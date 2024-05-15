using api.Data;
using api.Model.DTO;
using api.Models;
using static api.Services.PictureWorker;

namespace api.Services.DataServices
{
    public class PassportService(AppDbContext db) : DataServiceBase<PassportEntity, PassportDTO>(db)
    {
        public override async Task UpdateAsync(PassportEntity passportToUpdate, PassportDTO passportDTO)
        {
            passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.Scan);
            passportToUpdate.PersonId = passportDTO.Id;

            await _db.SaveChangesAsync();
        }
    }
}