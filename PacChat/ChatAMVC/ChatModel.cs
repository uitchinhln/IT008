﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PacChat.MVC;
using PacChat.Utils;
using PacChat.ChatPageContents.ViewModels;
using static PacChat.Utils.UserUtils;
using System.Windows.Controls;

namespace PacChat.ChatAMVC
{
    public class ChatModel : Model<ChatApplication>
    {
        public static List<string> FriendIDs { get; set; } = new List<string>();
        public static Dictionary<string, ShortProfile> FriendShortProfiles = new Dictionary<string, ShortProfile>();

        public string SelfID { get; set; }
 
        #region OnChatPage
        public string Title { get; set; } = "";

        public string previousSelectedConversation { get; set; } = "";
        public string currentSelectedConversation { get; set; } = "";
        public List<BubbleInfo> CurrentUserMessages { get; set; } = new List<BubbleInfo>();
        public Dictionary<string, ConversationBubble> Conversations { get; set; } = new Dictionary<string, ConversationBubble>();
        public Dictionary<string, string> PrivateConversations { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, UserControl> UserControls { get; set; } = new Dictionary<string, UserControl>();
        public Dictionary<string, bool> IsOnline { get; set; } = new Dictionary<string, bool>();
        #endregion

        #region OnSettingPage
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public int Gender { get; set; }
        #endregion
    }
}
