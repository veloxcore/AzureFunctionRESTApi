using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Rest.Data.Infrastructure
{
    public interface ITransactionHelper
    {
        TransactionScope StartTransaction();
    }
    public class TransactionHelper : ITransactionHelper
    {
        public TransactionScope StartTransaction()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}
