using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class DistributedCipherTest : IDistributedCipherTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            base.TestInitialize();

            IByteSetRepository byteSetRepository = MockRepository.GenerateMock<IByteSetRepository>();

            this.distributedCipherUnderTest = new DistributedCipher.Common.DistributedCipher(Guid.NewGuid(), byteSetRepository, this.header, this.ciphers, this.random);
        }
    }
}
