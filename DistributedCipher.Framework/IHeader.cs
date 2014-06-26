using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IHeader
    {
        IByteIndex ByteIndex { get; }
        IDistributedCipher DistributedCipher { set; }
        int EncryptedLength { get; }
        string Key { get; }

        int Decrypt(ref byte[] bytes);
        byte[] Encrypt(int indexOffset);
    }
}
