using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IGenericRepository<TEntity>:IAddableRepository<TEntity>
        ,IRemoveableRepository<TEntity>,IUpdateableRepository<TEntity>,IReadableRepository<TEntity>,ISaveChangesable<TEntity>
        where TEntity : class
    {

    }
}
