using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        void Create(T item);
        Task<T>? FindById(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> Get();
        IEnumerable<T> Get(Func<T, bool> predicate);
        void Remove(T item);
        void Update(T item);
        Task<IEnumerable<T>> GetByConditions(Dictionary<string, string> conditions);

    }
}
