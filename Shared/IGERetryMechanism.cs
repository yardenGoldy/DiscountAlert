using System;

namespace DiscountAlert.Shared
{
    public interface IGERetryMechanism
    {
        TResult InvokeFunc<TResult>(Delegate f, string methodName, int tries, params object[] args);
        TResult InvokeFunc<TResult>(ref object targetInstance, string methodName, int tries, params object[] args);
        TResult InvokeFunc<TTarget, TResult>(ref TTarget targetInstance, string methodName, int tries, params object[] args);
        bool InvokeAction(Delegate f, int tries, params object[] args);
        bool InvokeAction<TTarget>(Delegate f, int tries, params object[] args);
    }
}
