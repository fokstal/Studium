using api.Data;
using api.Model;
using api.Model.DTO;

using static api.Services.PictureWorker;

namespace api.Services.DataServices
{
    public class PassportService(AppDbContext db) : DataServiceBase<Passport, PassportDTO>(db)
    {
        public override async Task UpdateAsync(Passport passportToUpdate, PassportDTO passportDTO)
        {
            passportToUpdate.ScanFileName = await UploadPassportScanAsync(passportDTO.Scan);
            passportToUpdate.PersonId = passportDTO.Id;

            await _db.SaveChangesAsync();
        }
    }
}