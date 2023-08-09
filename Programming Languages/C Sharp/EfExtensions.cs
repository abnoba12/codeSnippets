using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace LCIMSSQLRepository.Extensions
{
    public static class EfExtensions
    {
        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query)
        {
            List<T> result = new List<T>(); 
            using (var scope = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }, 
                TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await query.ToListAsync();
                scope.Complete();
            }
            return result;
        }

        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query)
        {
            List<T> result = new List<T>();
            using (var scope = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }, 
                TransactionScopeAsyncFlowOption.Enabled))
            {
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }

        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            T result = default(T);
            using (var scope = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }, 
                TransactionScopeAsyncFlowOption.Enabled))
            {
                result = query.FirstOrDefault(expression);
                scope.Complete();
            }
            return result;
        }

        public static T FirstOrDefaultWithNoLock<T>(this IQueryable<T> query)
        {
            T result = default(T);
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                result = query.FirstOrDefault();
                scope.Complete();
            }
            return result;
        }

        public static bool AnyWithNoLock<T>(this IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var r = query.Any(expression);
                scope.Complete();
                return r;
            }
        }

        public static int CountWithNoLock<T>(this IQueryable<T> query, System.Linq.Expressions.Expression<Func<T, bool>> expression)
        {
            int result = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                result = query.Count(expression);
                scope.Complete();
            }
            return result;
        }
    }
}