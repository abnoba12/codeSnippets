using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LCIWebAPI.Business.Utility
{
    public class UnitOfWork
    {
        public static async Task Do(Func<Task> work, TransactionScopeOption scopeOption = TransactionScopeOption.Required, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            // start TransactionScope
            using (var scope = new TransactionScope(
                scopeOption,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await work.Invoke();
                scope.Complete();
            }
        }

        public static async Task<T> Do<T>(Func<Task<T>> work, TransactionScopeOption scopeOption = TransactionScopeOption.Required, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            // start TransactionScope
            using (var scope = new TransactionScope(
                scopeOption,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await work.Invoke();
                scope.Complete();
                return result;
            }
        }

        public static void Do(Action work, TransactionScopeOption scopeOption = TransactionScopeOption.Required, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            // start TransactionScope
            using (var scope = new TransactionScope(
                scopeOption,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                work.Invoke();
                scope.Complete();
            }
        }
    }
}