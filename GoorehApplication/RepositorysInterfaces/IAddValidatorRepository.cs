using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.RepositorysInterfaces
{
    public interface IAddValidatorRepository<TEntity>
    {
        Dictionary<string, string> AddErrorMessages { get; set; }
        bool AddValidate(TEntity entity);
    }
}
