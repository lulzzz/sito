using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServerSideAnalytics
{
    public interface IWebRequestRepository<T> where T : IWebRequest
    {
        T GetNew();
        Task AddAsync(T request);
        Task<long> CountAsync(DateTime from, DateTime to);
        Task<long> CountUniqueVisitorsAsync(DateTime from, DateTime to);
        Task<IEnumerable<IWebRequest>> QueryAsync(Expression<Func<T, bool>> where);
    }
}