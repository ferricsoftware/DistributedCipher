using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ByteIndexTest : IByteIndexTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            IList<byte> byteIndex = new List<byte>();
            byteIndex.Add(Convert.ToByte('A'));
            byteIndex.Add(Convert.ToByte('B'));
            byteIndex.Add(Convert.ToByte('C'));
            byteIndex.Add(Convert.ToByte('Z'));

            this.keyValueNotFound = new KeyValuePair<int, byte>(0, Convert.ToByte('X'));
            this.minimumIndex = new KeyValuePair<int, byte>(0, Convert.ToByte('A'));
            this.maximumIndex = new KeyValuePair<int, byte>(3, Convert.ToByte('Z'));

            this.byteIndexUnderTest = new ByteIndex(byteIndex);
        }
    }
}
