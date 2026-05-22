using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Interfaces
{
    public interface IAddValidatorRepository<TEntity>
    {
        Dictionary<string, string> AddErrorMessages { get; set; }
        void AddValidate(TEntity entity);
    }
}
