using System.Threading;
using System.Threading.Tasks;

namespace DiscountAlert.Shared
{
    public class GEThreadSleep : IGESleepService
    {
        public void SleepFor(int timeInMilliseconds)
        {
            Thread.Sleep(timeInMilliseconds);
        }

        public async Task SleepForAsync(int timeInMilliseconds)
        {
            await Task.Delay(1000);
        }
    }
}
