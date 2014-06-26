using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class DistributedCipherTest_As_Ferric_Node : IFerricNodeTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            base.TestInitialize();

            byte[] first = new byte[] { Convert.ToByte('c') };
            byte[] second = new byte[] { Convert.ToByte('f') };
            byte[] third = new byte[] { Convert.ToByte('h') };

            IList<ICipher> ciphers = new List<ICipher>();
            ciphers.Add(MockRepository.GenerateStub<ICipher>());
            ciphers[0].Stub(c => c.Length).Return(1);
            ciphers[0].Stub(c => c.EncryptedLength).Return(1);
            ciphers[0].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            ciphers[0].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[2]).Dummy)).Return(first);
            ciphers.Add(MockRepository.GenerateStub<ICipher>());
            ciphers[1].Stub(c => c.Length).Return(1);
            ciphers[1].Stub(c => c.EncryptedLength).Return(1);
            ciphers[1].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            ciphers[1].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[1]).Dummy)).Return(second);
            ciphers.Add(MockRepository.GenerateStub<ICipher>());
            ciphers[2].Stub(c => c.Length).Return(1);
            ciphers[2].Stub(c => c.EncryptedLength).Return(1);
            ciphers[2].Stub(c => c.Decrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);
            ciphers[2].Stub(c => c.Encrypt(ref Arg<byte[]>.Ref(Is.Anything(), new byte[0]).Dummy)).Return(third);

            IByteSetRepository byteSetRepository = MockRepository.GenerateStub<IByteSetRepository>();
            IHeader header = null;
            Guid id = Guid.NewGuid();
            Random random = new Random();

            this.ferricNodeUnderTest = new DistributedCipher.Common.DistributedCipher(id, byteSetRepository, header, ciphers, this.childFerricNode, random);
        }
    }
}
