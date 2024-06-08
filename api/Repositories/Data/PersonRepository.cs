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
            IEnumerable<PersonEntity> personList = await _db.Person.Include(personDb => personDb.Passport).Include(personDb => personDb.Student).ToArrayAsync();

            return personList;
        }

        public async override Task<PersonEntity?> GetAsync(int id)
        {
            PersonEntity? person = await _db.Person.Include(personDb => personDb.Passport).Include(personDb => personDb.Student).FirstOrDefaultAsync(personDb => personDb.Id == id);

            return person;
        }

        public async Task<PersonEntity?> GetAsync(string firstName, string middleName, string lastName)
        {
            PersonEntity? person =
            await _db.Person
                .Include(personDb => personDb.Passport)
                .Include(personDb => personDb.Student)
                .FirstOrDefaultAsync
                (
                    personDb =>
                        personDb.FirstName.ToLower() == firstName.ToLower() &&
                        personDb.MiddleName.ToLower() == middleName.ToLower() &&
                        personDb.LastName.ToLower() == lastName.ToLower()
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