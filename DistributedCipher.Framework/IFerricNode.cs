using System;

namespace DistributedCipher.Framework
{
    public interface IFerricNode
    {
        IFerricNode Child { get; }
        Guid ID { get; }
        IFerricNode Parent { get; set; }

        byte[] Decrypt(byte[] bytes);
        string Decrypt(string message);
        byte[] Encrypt(byte[] bytes);
        string Encrypt(string message);
    }
}
