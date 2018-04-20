using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleHelperCore.Interface;
using Microsoft.Extensions.Configuration;

namespace ConsoleHelperCore
{
    public class AppService : IAppService
    {
        private readonly IDbConnection _db;

        private readonly IConfigurationRoot _config;

        public AppService(IDbConnection db, IConfigurationRoot config)
        {
            _db = db;
            _config = config;
        }

        public async Task RunAsync(CancellationToken token)
        {
            var millisecondsDelay = _config.GetValue("millisecondsDelay", 1000);
            await Task.Delay(millisecondsDelay);
        }
    }
}
