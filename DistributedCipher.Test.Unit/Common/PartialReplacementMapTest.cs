using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class PartialReplacementMapTest : ICipherTypeTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            IByteMap byteMap = new ByteMap(FerricHelper.AlphaNumeric);

            this.cipherTypeUnderTest = new ReplacementMap(byteMap);
        }
    }
}
