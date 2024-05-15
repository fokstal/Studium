using api.Data;
using api.Models;
using api.Model.DTO;
using Microsoft.EntityFrameworkCore;

using static api.Services.PictureWorker;

namespace api.Services.DataServices
{
    public class PersonService(AppDbContext db) : DataServiceBase<PersonEntity, PersonDTO>(db)
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