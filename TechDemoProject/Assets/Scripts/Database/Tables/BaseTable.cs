﻿using System;
using MessagePack;

namespace Tech.DB
{
    //All Table will inherit from this class eventually
    public abstract class BaseTable
    {
        public byte[] Id { get; set; }
    }
}