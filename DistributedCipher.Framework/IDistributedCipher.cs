using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IDistributedCipher : IFerricNode
    {
        IByteSetRepository ByteSetRepository { get; }
        IList<ICipher> Ciphers { get; }
        IHeader Header { get; }
    }
}
