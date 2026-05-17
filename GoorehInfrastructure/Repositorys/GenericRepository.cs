using Azure;
using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehDomain.Entities.Base;
using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.Repositorys
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseThing
    {
        // co pilet pishnahad dad dbContext Kode porject ro estefadeh konam
        //private readonly DbContext _db;
        protected readonly GoorehDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(GoorehDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }


        public virtual void Validate(TEntity entity)
        {

        }
        public virtual async Task AddAsync(TEntity entity)
        {

            Validate(entity);
            await _dbSet.AddAsync(entity);
          //  await SaveAsync();
        }
        public void Update(TEntity entity)
        {
            Validate(entity);
            _dbSet.Update(entity);
           // _dbContext.SaveChanges();
        }


        public async virtual Task Delete(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {

                _dbSet.Remove(entity);
             //   await SaveAsync();
            }
        }

        public virtual IQueryable<TEntity> GetAll(int page = 0, int count = 10)
        {
            return _dbSet.Skip(page * count).Take(count);
        }
        public virtual IQueryable<TEntity> SimpleGetAll()
        {
            return _dbSet;
        }
        public virtual async Task<TEntity?> GetByGuidAsync(string guid)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Guid == guid);
        }
        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        //public virtual async Task SaveAsync() =>

        //    await _dbContext.SaveChangesAsync();

        public Task<int> SaveChangesAsync()
        {
           return _dbContext.SaveChangesAsync();
        }

  









        //public async Task<TEntity?> GetByIdAsync(int id)
        //=> await _db.FindAsync(s=>s.id);
    }
}
