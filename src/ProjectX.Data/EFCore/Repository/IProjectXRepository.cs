using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Data.EFCore
{
    public interface IProjectXRepository<TEntity, TPrimaryKey> where TEntity : class where TPrimaryKey: struct
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByPrimaryKeyAsync(TPrimaryKey input);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TPrimaryKey input);
    }
}
