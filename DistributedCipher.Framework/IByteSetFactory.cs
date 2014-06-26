using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IByteSetFactory
    {
        IByteIndex GenerateByteIndex(Guid id, byte[] data);
        IByteMap GenerateByteMap(Guid id, Dictionary<byte, byte> data);
    }
}
