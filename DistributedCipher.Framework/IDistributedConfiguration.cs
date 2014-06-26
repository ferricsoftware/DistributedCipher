using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IDistributedConfiguration
    {
        IList<ICipher> Ciphers { get; set; }
        IHeader Header { get; set; }
    }
}
