using System;
using DiscountAlert.Shared;

namespace DiscountAlert.ExceptionHandler
{
    public class GEExceptionHandler : IGEExceptionHandler
    {
        public string ExceptionHandler(System.Exception occured)
        {
            switch (occured.Message) {
                case "A specific known error message": 
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}
