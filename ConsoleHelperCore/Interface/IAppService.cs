﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleHelperCore.Interface
{
    public interface IAppService
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
