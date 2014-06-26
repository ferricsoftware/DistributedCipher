using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using DistributedCipher.Common;

namespace DistributedCipher.Test.Unit.Common
{
    [TestClass]
    public class ScramblerTest_As_Ferric_Node : IFerricNodeTest
    {
        [TestInitialize()]
        public override void TestInitialize()
        {
            Guid id = Guid.NewGuid();

            base.TestInitialize();

            IList<int> scrambledIndexes = new List<int>() { 0 };

            this.ferricNodeUnderTest = new Scrambler(id, scrambledIndexes, this.childFerricNode);
        }
    }
}
