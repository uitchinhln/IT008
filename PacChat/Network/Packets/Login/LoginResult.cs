﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using MaterialDesignThemes.Wpf;
using PacChat.MVC;
using PacChat.Resources.CustomControls.Dialogs;
using PacChat.Windows.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacChat.Network.Packets.Login
{
    public class LoginResult : IPacket
    {
        public int StatusCode { get; set; }
        public string Token { get; set; }

        public void Decode(IByteBuffer buffer)
        {
            StatusCode = buffer.ReadInt();
            Token = ByteBufUtils.ReadUTF8(buffer);
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            LoginApp loginApp = AppManager.GetAppOfType<LoginApp>() as LoginApp;
            if (loginApp == null) return;
            if (StatusCode == 200)
            {
                (session as ClientSession).LoggedIn(Token);
            }
            Application.Current.Dispatcher.Invoke(() => loginApp.view.LoginResponse(StatusCode));
            ChatConnection.Instance.OnResponse(StatusCode);
        }
    }
}
