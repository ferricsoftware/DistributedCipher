using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class HeaderTest_ThirtyOne_Ciphers_PreferredLength_Three : HeaderBaseTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            this.numberOfCiphers = 31;

            base.TestInitialize();

            IList<byte> bytes = new List<byte>(FerricHelper.Numbers);
            bytes.Add(Convert.ToByte('$'));
            IByteIndex byteIndex = new ByteIndex(bytes);

            this.headerUnderTest = new Header(byteIndex, 3);
            this.headerUnderTest.DistributedCipher = this.distributedCipher;
        }
    }
}
