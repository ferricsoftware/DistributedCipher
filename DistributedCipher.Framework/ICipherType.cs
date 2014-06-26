using System;

namespace DistributedCipher.Framework
{
    public interface ICipherType
    {
        string Name { get; set; }

        byte[] Decrypt(byte[] bytes);
        byte[] Encrypt(byte[] bytes);
    }
}
