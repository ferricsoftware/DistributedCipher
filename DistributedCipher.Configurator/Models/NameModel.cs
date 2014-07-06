using System;

namespace DistributedCipher.Configurator.Models
{
    public class NameModel
    {
        protected Guid id;
        protected string name;

        public Guid ID { get { return this.id; } }
        public string Name { get { return this.name; } set { this.name = value; } }

        public NameModel(Guid id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
