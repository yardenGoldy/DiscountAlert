using System;
using System.Threading;
using DiscountAlert.Shared;

namespace DiscountAlert.RetryMechanism
{
    
public class GERetryMechanism : IGERetryMechanism
    {
        public IGEExceptionHandler _exceptionHandler { get; set; }
        public GERetryMechanism(IGEExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        public TResult InvokeFunc<TResult>(ref object targetInstance, string methodName, int tries, params object[] args)
        {
            return InvokeFunc<object, TResult>(ref targetInstance, methodName, tries, 100, args);
        }
        public TResult InvokeFunc<TTarget, TResult>(ref TTarget targetInstance, string methodName, int tries, params object[] args)
        {
            return InvokeFunc<TTarget, TResult>(ref targetInstance, methodName, tries, 100, args);
        }
        public TResult InvokeFunc<TResult>(Delegate f, string methodName, int tries, params object[] args)
        {
            var target = f.Target;
            return InvokeFunc<object, TResult>(ref target, methodName, tries, 100, args);
        }
        public bool InvokeAction(Delegate f, int tries, params object[] args)
        {
            return InvokeAction(f, tries, 100, args);
        }
        public bool InvokeAction<TTarget>(Delegate f, int tries, params object[] args)
        {
            return InvokeAction(f, tries, 100, args);
        }

        private TResult InvokeFunc<TTarget, TResult>(ref TTarget targetInstance, string methodName, int tries, int delay, params object[] args)
        {
            // Assign default value for generic type.
            TResult result = default(TResult);

            for (int attemptNumber = 1; attemptNumber <= tries; attemptNumber++)
            {
                try
                {
                    //                 var result = f.DynamicInvoke(args);
                    // return (TResult)Convert.ChangeType(result, typeof(TResult));
                    // var targetType = ((TResult)f.Target).GetType();

                    // var targetMethod = targetType.GetMethod(f.Method.Name);
                    // var ddasdsd = targetMethod.Invoke(f.Target, new object[] { args[0] });
                    // var cast = (TResult)f.Target;
                    // Activator.CreateInstance(cast.GetType());
                    var method = targetInstance.GetType().GetMethod(methodName);
                    result = (TResult)method.Invoke(targetInstance, args);
                    break;
                }
                catch (Exception ex)
                {
                    _exceptionHandler.ExceptionHandler(ex);


                    // todo: Throw error if retry has failed.
                    if (attemptNumber == tries)
                    {
                        // todo: Throw specific max retries limit
                        throw ex;
                    }

                    Thread.Sleep(delay);
                }
            }

            return result;
        }

        private bool InvokeAction(Delegate f, int tries, int delay, params object[] args)
        {
            // Assign default value for generic type.
            bool invokedSuccessfully = false;

            for (int attemptNumber = 1; attemptNumber <= tries; attemptNumber++)
            {
                try
                {
                    var method = f.Method;
                    method.Invoke(f.Target, args);
                    invokedSuccessfully = true;
                    break;
                }
                catch (Exception ex)
                {
                    var knownError = _exceptionHandler.ExceptionHandler(ex);

                    if (knownError != null)
                    {
                        return true;
                    }

                    // todo: Throw error if retry has failed.
                    if (attemptNumber == tries)
                    {
                        // todo: Throw specific max retries limit
                        throw ex;
                    }

                    Thread.Sleep(delay);
                }
            }

            return invokedSuccessfully;
        }
    }
}

