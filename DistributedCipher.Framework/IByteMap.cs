using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IByteMap : IByteSet
    {
        Dictionary<byte, byte> Data { get; }

        byte Lookup(byte source);
        byte ReverseLookup(byte source);
    }
}
