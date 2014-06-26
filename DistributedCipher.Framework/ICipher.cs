using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface ICipher
    {
        ICipherType CipherType { get; }
        int EncryptedLength { get; }
        int Length { get; }
         
        byte[] Decrypt(ref byte[] bytes);
        byte[] Encrypt(ref byte[] bytes);
    }
}
