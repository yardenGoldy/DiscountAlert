using System;
using DiscountAlert.Shared;

namespace DiscountAlert.ExceptionHandler
{
    public class GEExceptionHandler : IGEExceptionHandler
    {
        public string ExceptionHandler(System.Exception occured)
        {
            var currEx = occured;
            while (currEx != null)
            {
                switch (occured.Message)
                {
                    case "A specific known error message":
                        break;
                    case "A exception with a null response was thrown sending an HTTP request to the remote WebDriver server for URL http://localhost:40791/session/e8a5e94d1af10b8afa105401d6d6490a/element/a8482957-ce96-464d-80a1-f994b6ddd9e8/click. The status of the exception was UnknownError, and the message was: An error occurred while sending the request. The response ended prematurely.":
                        return "ignore";
                    default:
                        break;
                }

                currEx = currEx.InnerException;
            }

            return null;
        }
    }
    
}
