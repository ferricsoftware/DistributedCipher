using System;
using System.Collections.Generic;
using System.Data.Linq;

using DistributedCipher.Framework;

namespace DistributedCipher.ByteSetRepository.Memory
{
    public class ByteSetMemoryRepository : IByteSetRepository
    {
        protected IByteSetFactory byteSetFactory;
        protected Dictionary<Guid, IByteSet> byteSets;

        public ByteSetMemoryRepository(IByteSetFactory byteSetFactory)
        {
            this.byteSetFactory = byteSetFactory;

            this.byteSets = new Dictionary<Guid, IByteSet>();
        }

        public void Delete(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            if (!this.byteSets.ContainsKey(byteSet.ID))
                throw new KeyNotFoundException(byteSet.ID.ToString());

            this.byteSets.Remove(byteSet.ID);
        }

        public Guid Exists(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            if (this.byteSets.ContainsKey(byteSet.ID))
                return byteSet.ID;

            foreach (Guid key in this.byteSets.Keys)
            {
                IByteSet storedByteSet = this.byteSets[key];

                if (storedByteSet is IByteIndex && storedByteSet is IByteIndex)
                {
                    IByteIndex byteIndex = (IByteIndex)byteSet;
                    IByteIndex storedByteIndex = (IByteIndex)storedByteSet;

                    bool isMatching = true;
                    if (byteIndex.Data.Count == storedByteIndex.Data.Count)
                    {
                        for (int i = 0; i < byteIndex.Data.Count; i++)
                        {
                            if (byteIndex.Data[i] != storedByteIndex.Data[i])
                            {
                                isMatching = false;

                                break;
                            }
                        }
                    }

                    if (isMatching)
                        return storedByteIndex.ID;
                }
                else if (storedByteSet is IByteMap && storedByteSet is IByteMap)
                {
                    IByteMap byteMap = (IByteMap)byteSet;
                    IByteMap storedByteMap = (IByteMap)storedByteSet;

                    bool isMatching = true;
                    if (byteMap.Data.Count == storedByteMap.Data.Count)
                    {
                        foreach(byte byteKey in byteMap.Data.Keys)
                        {
                            if (!storedByteMap.Data.ContainsKey(byteKey) || byteMap.Data[byteKey] != storedByteMap.Data[byteKey])
                            {
                                isMatching = false;

                                break;
                            }
                        }
                    }

                    if (isMatching)
                        return storedByteMap.ID;
                }
            }

            return Guid.Empty;
        }

        public IByteSet Find(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID is empty.");

            if (this.byteSets.ContainsKey(id))
                return this.byteSets[id];

            return null;
        }

        public void Save(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            if (Exists(byteSet) != Guid.Empty)
                throw new DuplicateKeyException(byteSet.ID);

            this.byteSets[byteSet.ID] = byteSet;
        }
    }
}

