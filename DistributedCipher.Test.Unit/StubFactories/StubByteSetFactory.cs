using System;
using System.Collections.Generic;

using Rhino.Mocks;

using DistributedCipher.Framework;

namespace DistributedCipher.Test.Unit.StubFactories
{
    public class StubByteSetFactory : IByteSetFactory
    {
        public IByteIndex GenerateByteIndex(Guid id, byte[] data)
        {
            IByteIndex byteIndex = MockRepository.GenerateStub<IByteIndex>();

            byteIndex.Stub(bi => bi.ID).Return(id);
            byteIndex.Stub(bi => bi.Data).Return(data);

            return byteIndex;
        }

        public IByteMap GenerateByteMap(Guid id, Dictionary<byte, byte> data)
        {
            IByteMap byteMap = MockRepository.GenerateStub<IByteMap>();

            byteMap.Stub(bi => bi.ID).Return(id);
            byteMap.Stub(bi => bi.Data).Return(data);

            return byteMap;
        }
    }
}
