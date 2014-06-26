using System;

namespace DistributedCipher.Framework
{
    public interface IByteSetRepository
    {
        void Delete(IByteSet byteSet);
        Guid Exists(IByteSet byteSet);
        IByteSet Find(Guid id);
        void Save(IByteSet byteSet);
    }
}
