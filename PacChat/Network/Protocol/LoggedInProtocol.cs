﻿using PacChat.Network.Packets.AfterLoginRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChat.Network.Protocol
{
    public class LoggedInProtocol : PacChatProtocol
    {
        public LoggedInProtocol() : base("LoggedIn Protocol")
        {
            Inbound(0x00, new GetFriendIDsResult());
            Outbound(0x00, new GetFriendIDs());

            Inbound(0x01, new GetInfoResult());
            Outbound(0x01, new GetInfo());
        }
    }
}