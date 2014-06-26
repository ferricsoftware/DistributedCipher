using System;

namespace DistributedCipher.Framework
{
    public interface IRandomDataInserts : ICipher
    {
        IByteIndex ByteIndex { get; }
    }
}
