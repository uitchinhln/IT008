﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacChat.MessageCore.Message
{
    public class TextMessage : AbstractMessage
    {
        public string Message { get; set; }
    }
}