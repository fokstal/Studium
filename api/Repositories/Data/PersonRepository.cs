using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

using static api.Repositories.PictureRepository;

namespace api.Repositories.Data
{
    public class PersonRepository(AppDbContext db) : DataRepositoryBase<PersonEntity, PersonDTO>(db)
    {
        public async override Task<IEnumerable<PersonEntity>> GetListAsync()
        {
            IEnumerable<PersonEntity> personList = await _db.Person.Include(personDb => personDb.PassportEntity).Include(personDb => personDb.StudentEntity).ToArrayAsync();

            return personList;
        }

        public async override Task<PersonEntity?> GetAsync(int id)
        {
            PersonEntity? person = await _db.Person.Include(personDb => personDb.PassportEntity).Include(personDb => personDb.StudentEntity).FirstOrDefaultAsync(personDb => personDb.Id == id);

            return person;
        }

        public async Task<PersonEntity?> GetAsync(string firstName, string middleName, string lastName)
        {
            PersonEntity? person =
            await _db.Person
                .Include(personDb => personDb.PassportEntity)
                .Include(personDb => personDb.StudentEntity)
                .FirstOrDefaultAsync
                (
                    personDb =>
                        StringComparer.CurrentCultureIgnoreCase.Compare(personDb.FirstName, firstName) == 0 &&
                        StringComparer.CurrentCultureIgnoreCase.Compare(personDb.MiddleName, middleName) == 0 &&
                    StringComparer.CurrentCultureIgnoreCase.Compare(personDb.LastName, lastName) == 0
                );

            return person;
        }

        public override PersonEntity Create(PersonDTO personDTO)
        {
            return new()
            {
                FirstName = personDTO.FirstName,
                MiddleName = personDTO.MiddleName,
                LastName = personDTO.LastName,
                BirthDate = personDTO.BirthDate,
                Sex = personDTO.Sex,
                AvatarFileName = UploadPersonAvatarAsync(personDTO.Avatar, personDTO.Sex).Result,
            };
        }

        public async override Task UpdateAsync(PersonEntity personToUpdate, PersonDTO personDTO)
        {
            personToUpdate.FirstName = personDTO.FirstName;
            personToUpdate.MiddleName = personDTO.MiddleName;
            personToUpdate.LastName = personDTO.LastName;
            personToUpdate.BirthDate = personDTO.BirthDate;
            personToUpdate.Sex = personDTO.Sex;
            personToUpdate.AvatarFileName = await UploadPersonAvatarAsync(personDTO.Avatar, personToUpdate.Sex);

            await _db.SaveChangesAsync();
        }
    }
}