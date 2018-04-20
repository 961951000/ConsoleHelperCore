using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleHelperCore.Helpers
{
    public class LimitExecutor
    {
        private readonly SemaphoreSlim _semaphoreSlim;

        public LimitExecutor(int maxCount)
        {
            _semaphoreSlim = new SemaphoreSlim(maxCount, maxCount);
        }

        public async Task<T> Excute<T>(Func<T> func) => await Task.Run(() => func.Invoke());

        public async Task<T> Excute<T>(Func<Task<T>> func)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                return await Task.Run(() => func.Invoke());
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
