using Azure;
using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities;
using GoorehDomain.Entities.Base;
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





        public async virtual Task Delete(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {

                _dbSet.Remove(entity);

            }
        }

        public virtual IQueryable<TEntity> GetAll(int page = 0, int count = 10)
        {
            return _dbSet.Skip(page * count).Take(count);
        }


        public virtual IQueryable<TEntity> SimpleGetAll()
        {
            return _dbSet.AsQueryable();
        }


        public virtual async Task<TEntity?> GetByGuidAsync(string guid)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Guid == guid);
        }


        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }



        public void Update(TEntity entity)
        {
            //Validate(entity);
            //code class
            _dbSet.Update(entity);

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
