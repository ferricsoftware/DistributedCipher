using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IByteIndex : IByteSet
    {
        IList<byte> Data { get; }

        byte Lookup(int index);
        int ReverseLookup(byte source);
    }
}
