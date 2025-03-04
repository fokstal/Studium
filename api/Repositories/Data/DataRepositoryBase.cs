using api.Data;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Data
{
    public abstract class DataRepositoryBase<TValue, TValueDTO>(AppDbContext db) 
        where TValue : ModelEntity
        where TValueDTO : class
    {
        protected readonly AppDbContext _db = db;

        public virtual async Task<bool> CheckExistsAsync(int id) 
            => await _db.Set<TValue>().FirstOrDefaultAsync(valueDb => valueDb.Id == id) is not null;

        public virtual async Task<IEnumerable<TValue>> GetListAsync()
        {
            IEnumerable<TValue> valueList = await _db.Set<TValue>().ToArrayAsync();

            return valueList;
        }

        public virtual async Task<TValue?> GetAsync(int passportEntityId) 
            => await _db.Set<TValue>().FirstOrDefaultAsync(valueDb => valueDb.Id == passportEntityId);

        public abstract TValue Create(TValueDTO valueDTO);

        public virtual async Task AddAsync(TValue valueToAdd)
        {
            await _db.Set<TValue>().AddAsync(valueToAdd);

            await _db.SaveChangesAsync();
        }

        public abstract Task UpdateAsync(TValue valueToUpdate, TValueDTO valueDTO);

        public virtual async Task RemoveAsync(TValue valueToRemove)
        {
            _db.Set<TValue>().Remove(valueToRemove);

            await _db.SaveChangesAsync();
        }
    }
}