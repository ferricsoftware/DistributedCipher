using System;
using System.Collections.Generic;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class ByteSetFactory : IByteSetFactory
    {
        public IByteIndex GenerateByteIndex(Guid id, byte[] data)
        {
            return new ByteIndex(id, data);
        }

        public IByteMap GenerateByteMap(Guid id, Dictionary<byte, byte> data)
        {
            return new ByteMap(id, data);
        }
    }
}
