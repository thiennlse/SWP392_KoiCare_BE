using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBaseRepository <T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        IQueryable<T> GetQueryable(CancellationToken cancellationToken = default);
        void add (T entity);
        void update (T entity);
        void delete (T entity);
        void CheckCancellationToken(CancellationToken cancellationToken = default);
    }
}
