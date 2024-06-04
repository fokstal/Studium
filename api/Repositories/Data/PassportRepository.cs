using api.Data;
using api.Model.DTO;
using api.Models;

using static api.Repositories.PictureRepository;

namespace api.Repositories.Data
{
    public class PassportRepository(AppDbContext db) : DataRepositoryBase<PassportEntity, PassportDTO>(db)
    {
        private readonly PersonRepository _personRepository = new(db);
        public override PassportEntity Create(PassportDTO passportDTO)
        {
            PersonEntity personEntity =
                _personRepository.GetAsync(personEntityId: passportDTO.PersonEntityId).Result
                ?? throw new Exception("Person on Passport is null!");

            return new()
            {
                ScanFileName =
                    UploadPassportScanAsync(passportDTO.ScanFile, personEntity.FirstName + personEntity.AvatarFileName).Result,
                    
                PersonEntityId = passportDTO.PersonEntityId,
            };
        }

        public override async Task UpdateAsync(PassportEntity passportEntityToUpdate, PassportDTO passportDTO)
        {
            PersonEntity personEntity =
                _personRepository.GetAsync(personEntityId: passportDTO.PersonEntityId).Result
                ?? throw new Exception("Person on Passport is null!");

            passportEntityToUpdate.ScanFileName = await
                UploadPassportScanAsync(passportDTO.ScanFile, personEntity.FirstName + personEntity.AvatarFileName);

            passportEntityToUpdate.PersonEntityId = passportDTO.Id;

            await _db.SaveChangesAsync();
        }
    }
}