using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IAddableRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity); //creat  
        //Task AddRangeAsync(IEnumerable<TEntity> entity);
        //Task<int> SaveChangesAsync();

    }
}
