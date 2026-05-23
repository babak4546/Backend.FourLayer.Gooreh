using Azure;
using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehDomain.Entities.Base;
using GoorehDomain.Interfaces;
using GoorehInfrastructure.DbContextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.Repositorys
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseThing
    {
        public Dictionary<string, string> AddErrorMessages { get; set; } = new Dictionary<string, string>();

        //public virtual bool AddValidate(TEntity entity)
        //{
        //megdar pishfarz false hast baraye service haee keh
        //az generic repository estefadeh mikonan nah repository khode entity
        //pishfarz megdar true hast 
        //    return true;
        //}

        // co pilet pishnahad dad dbContext Kode porject ro estefadeh konam
        //private readonly DbContext _db;
        protected readonly GoorehDbContext _dbContext;

        public GenericRepository(GoorehDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        protected readonly DbSet<TEntity> _dbSet;


        //same

        public virtual async Task AddAsync(TEntity entity)
        {
            // in kar nemikoneh chone dareh ba entity check mikoneh keh aya on intity in interface ro piadeh sazi 
            // kardeh ya na va ba repository haye farzand check nemi koneh 
            //if (entity is IAddValidatorRepository<TEntity> entityValid)
            //{
            //    entityValid.AddValidate(entity);
            //    //await _dbSet.AddAsync(entity);

            //}
            //    await _dbSet.AddAsync(entity);


            // alan addValidate hamisheh bayad ejra besheh 
            //same as class vid
            if (this is IAddValidatorRepository<TEntity>)
            {
                if (((IAddValidatorRepository<TEntity>)this).AddValidate(entity))
                {

                    await _dbSet.AddAsync(entity);

                }
                else
                {
                    throw new Exception($"Invalid{entity}");
                }
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }

        }

        //same as class vid
        public virtual async Task Delete(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                if (entity is IVirtualRemove)
                {
                    ((IVirtualRemove)entity).IsRemoved = true;
                    _dbSet.Update(entity);
                }
                else
                {
                    _dbSet.Remove(entity);
                }

            }
        }



        public virtual IQueryable<TEntity> GetAll(int page = 0, int count = 10)
        {
            return _dbSet.Skip(page * count).Take(count);
        }
        //same as class vid

        public virtual IQueryable<TEntity> SimpleGetAll()
        {
            var query = _dbSet.AsQueryable();
            if (typeof(IVirtualRemove).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(e => ((IVirtualRemove)e).IsRemoved == false);
            }
            return query;
        }
        //same as class vid

        public virtual async Task<TEntity?> GetByGuidAsync(string guid)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Guid == guid);
        }

        //same as class vid

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == id);
        }


        //same as class vid
        public virtual void Update(TEntity entity)
        {
            if (this is IAddValidatorRepository<TEntity>)
            {
                if (((IAddValidatorRepository<TEntity>)this).AddValidate(entity))
                {
                    _dbSet.Update(entity);
                }
                else
                {
                    throw new Exception($"Invalid {entity}");
                }
            }
            //Validate(entity);
            //code class
            else
            {
                _dbSet.Update(entity);

            }

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
