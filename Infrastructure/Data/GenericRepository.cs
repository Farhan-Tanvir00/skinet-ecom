using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoredContext storedContext;

        public GenericRepository(StoredContext StoredContext)
        {
            storedContext = StoredContext;
        }
        public void Add(T entity)
        {
            storedContext.Set<T>().Add(entity);
        }

        public bool Exists(int Id)
        {
            return storedContext.Set<T>().Any(x => x.Id == Id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await storedContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpec(BaseSpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            return await storedContext.Set<T>().FindAsync(Id);
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
             storedContext.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await storedContext.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            storedContext.Set<T>().Attach(entity);
            storedContext.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(storedContext.Set<T>().AsQueryable(), spec);
        }
    }
}
