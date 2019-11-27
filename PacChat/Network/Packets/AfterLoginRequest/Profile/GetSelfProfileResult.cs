﻿using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using PacChat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChat.Network.Packets.AfterLoginRequest.Profile
{
    public class GetSelfProfileResult : IPacket
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Town { get; set; } = "Default";
        public String Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }


        public void Decode(IByteBuffer buffer)
        {
            FirstName = ByteBufUtils.ReadUTF8(buffer);
            LastName = ByteBufUtils.ReadUTF8(buffer);
            Town = ByteBufUtils.ReadUTF8(buffer);
            Email = ByteBufUtils.ReadUTF8(buffer);

            // Please send D.O.B as 3 integers representing <year, month, day>
            DateOfBirth = new DateTime(year:buffer.ReadInt(), month:buffer.ReadInt(), day:buffer.ReadInt());

            Gender = (Gender)buffer.ReadInt();
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            // Display information on setting tab
        }
    }
}