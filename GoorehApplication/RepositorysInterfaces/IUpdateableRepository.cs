using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IUpdateableRepository<TEntity>where TEntity : class
    {
        void Update(TEntity entity);
        //Task<int> SaveChangesAsync();


    }
}
