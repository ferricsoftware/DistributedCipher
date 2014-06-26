using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class CipherTest_Length_Of_One_EncryptedLength_Of_Two : ICipherTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            base.TestInitialize();

            this.cipherType.Stub(ct => { ct.Encrypt(Arg<byte[]>.Is.Anything); })
                .Return(new byte[] { Convert.ToByte('a'), Convert.ToByte('b') });

            this.cipherUnderTest = new Cipher(this.cipherType);
        }
    }
}
