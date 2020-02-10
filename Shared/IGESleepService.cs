using System;
using System.Threading.Tasks;

namespace DiscountAlert.Shared
{
    public interface IGESleepService
    {
        void SleepFor(int timeInMilliseconds);

        Task SleepForAsync(int timeInMilliseconds);
    }
}
