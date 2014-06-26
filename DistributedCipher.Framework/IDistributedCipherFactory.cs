using System;

namespace DistributedCipher.Framework
{
    public interface IDistributedCipherFactory
    {
        ICipher GenerateCipher(ICipherType cipherType);
    }
}
