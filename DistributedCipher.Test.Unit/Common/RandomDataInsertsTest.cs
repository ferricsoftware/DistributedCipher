using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class RandomDataInsertsTest : IRandomDataInsertsTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            IByteIndex byteIndex = new ByteIndex(FerricHelper.Letters);

            this.randomDataUnderTest = new RandomDataInserts(byteIndex);
        }
    }
}
