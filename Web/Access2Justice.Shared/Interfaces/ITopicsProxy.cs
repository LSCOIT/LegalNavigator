using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsProxy
    {
        Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate);
    }
}
