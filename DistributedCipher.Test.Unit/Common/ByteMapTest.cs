using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

using DistributedCipher.Common;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ByteMapTest : IByteMapTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Dictionary<byte, byte> byteMap = new Dictionary<byte, byte>();
            byteMap[Convert.ToByte('A')] = Convert.ToByte('B');
            byteMap[Convert.ToByte('B')] = Convert.ToByte('C');
            byteMap[Convert.ToByte('C')] = Convert.ToByte('Z');
            byteMap[Convert.ToByte('Z')] = Convert.ToByte('A');

            this.lookupKey = new KeyValuePair<byte, byte>(Convert.ToByte('A'), Convert.ToByte('B'));
            this.lookupKeyNotFound = new KeyValuePair<byte, byte>(Convert.ToByte('-'), Convert.ToByte(0));
            this.reverseLookupKey = new KeyValuePair<byte, byte>(Convert.ToByte('A'), Convert.ToByte('Z'));
            this.reverseLookupKeyNotFound = new KeyValuePair<byte, byte>(Convert.ToByte('-'), Convert.ToByte(0));

            this.byteMapUnderTest = new ByteMap(byteMap);
        }
    }
}
