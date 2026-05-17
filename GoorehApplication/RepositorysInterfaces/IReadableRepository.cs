using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IReadableRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByGuidAsync(string guid);
        Task<TEntity?> GetByIdAsync(int id);
        IQueryable<TEntity> GetAll(int page, int count);
        IQueryable<TEntity> SimpleGetAll();

    }
}
