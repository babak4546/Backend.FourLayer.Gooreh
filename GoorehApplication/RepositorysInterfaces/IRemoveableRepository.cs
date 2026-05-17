using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IRemoveableRepository<TEntity> where TEntity : class
    {
        Task Delete(int id);
        //Task<int> SaveChangesAsync();


    }
}
