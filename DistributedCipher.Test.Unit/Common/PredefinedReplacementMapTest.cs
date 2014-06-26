using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class PredefinedReplacementMapTest : ICipherTypeTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            Dictionary<byte, byte> characterMap = new Dictionary<byte, byte>();
            characterMap[Convert.ToByte('a')] = Convert.ToByte('b');
            characterMap[Convert.ToByte('b')] = Convert.ToByte('c');
            characterMap[Convert.ToByte('c')] = Convert.ToByte('a');

            IByteMap byteMap = new ByteMap(characterMap);

            this.cipherTypeUnderTest = new ReplacementMap(byteMap);
        }
    }
}
