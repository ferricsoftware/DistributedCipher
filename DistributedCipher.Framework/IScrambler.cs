using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IScrambler : IFerricNode
    {
        IList<int> ScrambledIndexes { get; }
    }
}
