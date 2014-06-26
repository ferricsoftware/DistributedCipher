using System;
using System.Collections.Generic;

namespace DistributedCipher.Framework
{
    public interface IFerricNodeRepository
    {
        void Delete(IFerricNode root);
        Guid Exists(IFerricNode root);
        IFerricNode Find(Guid id);
        void Save(IFerricNode root);
    }
}
