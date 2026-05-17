using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface ISaveChangesable<TEntity> where TEntity : class
    {
        Task<int> SaveChangesAsync();

    }
}
