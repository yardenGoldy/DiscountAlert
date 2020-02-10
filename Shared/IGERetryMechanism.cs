using System;

namespace DiscountAlert.Shared
{
    public interface IGERetryMechanism
    {
        TResult InvokeFunc<TResult>(Delegate f, int retryNumber, params object[] args);
        bool InvokeAction(Delegate f, int retryNumber, params object[] args);
    }
}
