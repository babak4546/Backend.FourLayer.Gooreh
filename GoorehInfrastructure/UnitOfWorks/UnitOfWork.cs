using GoorehApplication.RepositorysInterfaces;
using GoorehApplication.UnitOfWorkInterfaces;
using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehInfrastructure.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : Thing
        {
            throw new NotImplementedException();
        }
    }
}
