using System;
namespace DiscountAlert.Shared
{
    public interface IGEExceptionHandler
    {
        string ExceptionHandler(Exception occured);
    }
}