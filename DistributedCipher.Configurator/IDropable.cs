using System;

namespace DistributedCipher.Configurator
{
    public interface IDropable
    {
        Type DataType { get; }

        void Drop(object data, int index = -1);
    }
}
