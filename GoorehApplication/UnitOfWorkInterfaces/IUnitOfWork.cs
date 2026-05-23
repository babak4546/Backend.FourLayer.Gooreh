using GoorehApplication.RepositorysInterfaces;
using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.UnitOfWorkInterfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity:Thing;

    }
}
