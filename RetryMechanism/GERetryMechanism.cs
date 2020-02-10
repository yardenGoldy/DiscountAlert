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

        public TResult InvokeFunc<TResult>(Delegate f, int tries, params object[] args)
        {
            return InvokeFunc<TResult>(f, tries, 100, args);
        }

        private TResult InvokeFunc<TResult>(Delegate f, int tries, int delay, params object[] args)
        {
            // Assign default value for generic type.
            TResult result = default(TResult);

            for (int attemptNumber = 1; attemptNumber <= tries; attemptNumber++)
            {
                try
                {
                    var resultAsObject = f.DynamicInvoke(args);
                    result = (TResult)Convert.ChangeType(resultAsObject, typeof(TResult));
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

        public bool InvokeAction(Delegate f, int tries, params object[] args)
        {
            return InvokeAction(f, tries, 100, args);
        }

        private bool InvokeAction(Delegate f, int tries, int delay, params object[] args)
        {
            // Assign default value for generic type.
            bool invokedSuccessfully = false;

            for (int attemptNumber = 1; attemptNumber <= tries; attemptNumber++)
            {
                try
                {
                    var resultAsObject = f.DynamicInvoke(args);
                    invokedSuccessfully = true;
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

            return invokedSuccessfully;
        }
    }
}

