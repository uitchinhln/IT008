﻿using DotNetty.Codecs.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Network.RestfulServer.Alias
{
    public interface IAliasExecutor
    {
        void Execute(IDictionary<string, List<string>> parameters, IFullHttpRequest request, IFullHttpResponse response);
    }
}