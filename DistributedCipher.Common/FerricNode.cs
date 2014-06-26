using System;

using DistributedCipher.Framework;

namespace DistributedCipher.Common
{
    public class FerricNode : IFerricNode
    {
        protected IFerricNode child;
        protected Guid id;
        protected IFerricNode parent;

        public IFerricNode Child { get { return this.child; } }
        public Guid ID { get { return this.id; } }
        public IFerricNode Parent { get { return this.parent; } set { this.parent = value; } }

        public FerricNode(Guid id)
        {
            this.id = id;
        }

        public FerricNode(Guid id, IFerricNode child)
            : this(id)
        {
            this.child = child;

            if (this.child != null)
                this.child.Parent = this;
        }

        public virtual byte[] Decrypt(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public virtual string Decrypt(string message)
        {
            byte[] bytes = FerricHelper.ConvertToByteArray(message);

            byte[] decryptedBytes = Decrypt(bytes);

            string decryptedMessage = FerricHelper.ConvertToString(decryptedBytes);

            return decryptedMessage;
        }

        public virtual byte[] Encrypt(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public virtual string Encrypt(string message)
        {
            byte[] bytes = FerricHelper.ConvertToByteArray(message);

            byte[] encryptedBytes = Encrypt(bytes);

            string encryptedMessage = FerricHelper.ConvertToString(encryptedBytes);

            return encryptedMessage;
        }
    }
}
