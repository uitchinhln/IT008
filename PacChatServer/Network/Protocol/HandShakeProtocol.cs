﻿using PacChatServer.Network.Packets.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer.Network.Protocol
{
    public class HandShakeProtocol : PacChatProtocol
    {
        public HandShakeProtocol() : base("HandShake")
        {
            Inbound(0x00, new PingReceive());
            Outbound(0x00, new PingRespone());
        }
    }
}