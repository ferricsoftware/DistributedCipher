using System;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class DistributedCipherFactory : IDistributedCipherFactory
    {
        protected IByteSetRepository byteSetRepository;
        protected IXmlFactory xmlFactory;

        public DistributedCipherFactory(IByteSetRepository byteSetRepository, IXmlFactory xmlFactory)
        {
            this.byteSetRepository = byteSetRepository;
            this.xmlFactory = xmlFactory;
        }

        public ICipher GenerateCipher(ICipherType cipherType)
        {
            return new Cipher(cipherType);
        }
    }
}
