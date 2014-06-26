using System;

namespace DistributedCipher.Framework
{
    public interface IByteSet
    {
        int Count { get; }
        Guid ID { get; }
    }
}
