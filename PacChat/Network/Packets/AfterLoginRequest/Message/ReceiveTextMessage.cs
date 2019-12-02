﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using PacChat.MessageCore.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacChat.Network.Packets.AfterLoginRequest.Message
{
    public class ReceiveTextMessage : IPacket
    {
        public string SenderID { get; set; }
        public TextMessage Message { get; set; } = new TextMessage();

        public void Decode(IByteBuffer buffer)
        {
            SenderID = ByteBufUtils.ReadUTF8(buffer);
            Message.Message = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            return byteBuf;
        }

        public void Handle(ISession session)
        {
            Console.WriteLine("Received");

            var app = MainWindow.chatApplication;
            app.model.ContactsMessages[SenderID].Add(new Utils.BubbleInfo(Message.Message, true));

            if (app.model.currentSelectedUser.CompareTo(SenderID) == 0)
                Application.Current.Dispatcher.Invoke(() => ChatPage.Instance.SendLeftMessages(Message, false));
        }
    }
}