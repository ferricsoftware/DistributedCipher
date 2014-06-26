using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.ByteSetRepository.Memory;
using DistributedCipher.Framework;
using DistributedCipher.Test.Unit.StubFactories;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ByteSetMemoryRepositoryTest : IByteSetRepositoryTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            IByteSetFactory byteSetFactory = new StubByteSetFactory();

            this.byteSetRepositoryUnderTest = new ByteSetMemoryRepository(byteSetFactory);
        }
    }
}
