using System;
using System.Collections.Generic;
using System.Data.Linq;

using DistributedCipher.Framework;

namespace DistributedCipher.FerricNodeRepository.Memory
{
    public class FerricNodeMemoryRepository : IFerricNodeRepository
    {
        protected Dictionary<Guid, IFerricNode> ferricNodes;

        public FerricNodeMemoryRepository()
        {
            this.ferricNodes = new Dictionary<Guid, IFerricNode>();
        }

        public void Delete(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Ferric Node");

            if (!this.ferricNodes.ContainsKey(ferricNode.ID))
                throw new KeyNotFoundException(ferricNode.ID.ToString());

            this.ferricNodes.Remove(ferricNode.ID);
        }

        public Guid Exists(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Ferric Node");

            if (this.ferricNodes.ContainsKey(ferricNode.ID))
                return ferricNode.ID;

            return Guid.Empty;
        }

        public IFerricNode Find(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID is empty.");

            if (this.ferricNodes.ContainsKey(id))
                return this.ferricNodes[id];

            return null;
        }

        public void Save(IFerricNode ferricNode)
        {
            if (ferricNode == null)
                throw new ArgumentNullException("Ferric Node");

            if (this.ferricNodes.ContainsKey(ferricNode.ID))
                throw new DuplicateKeyException(ferricNode.ID);

            this.ferricNodes[ferricNode.ID] = ferricNode;
        }
    }
}