﻿using PacChatServer.Entity;
using PacChatServer.IO.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChatServer
{
    public class ChatUserManager
    {
        public static ConcurrentDictionary<Guid, ChatUser> OnlineUsers { get; private set; } = new ConcurrentDictionary<Guid, ChatUser>();

        public static ChatUser LoadUser(Guid id)
        {
            if (OnlineUsers.ContainsKey(id))
            {
                return OnlineUsers[id];
            }

            ChatUser result = null;

            try
            {
                result = new ChatUserStore().Load(id);
            } catch (Exception e)
            {
                PacChatServer.GetServer().Logger.Error(e);
            }

            return result;
        }

        public static void MakeOnline(ChatUser user)
        {
            if (!OnlineUsers.ContainsKey(user.ID)) {
                OnlineUsers.TryAdd(user.ID, user);
                user.Online();
            }
        }

        public static void MakeOffline(ChatUser user)
        {
            if (OnlineUsers.ContainsKey(user.ID))
            {
                OnlineUsers.TryRemove(user.ID, out user);
                user.Offline();
            }
        }
    }
}
