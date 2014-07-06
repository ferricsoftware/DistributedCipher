using System;
using System.ComponentModel;
using System.Web.Services;

using DistributedCipher.Common;
using DistributedCipher.FerricNodeRepository.Cache;
using DistributedCipher.FerricNodeRepository.Xml;
using DistributedCipher.FerricNodeRepository.Memory;
using DistributedCipher.Framework;

namespace DistributedCipher.WebServices.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FerricNodes : WebService, IFerricNodeRepository
    {
        protected IFerricNodeRepository ferricNodeRepository;

        public FerricNodes()
        {
            IByteSetRepository byteSetRepository = new ByteSets();
            string fileName = Application["FerricNodeXmlFileName"].ToString();
            IXmlFactory xmlFactory = new XmlFactory();

            IFerricNodeRepository memoryRepository = new FerricNodeMemoryRepository();
            IFerricNodeRepository xmlRepository = new FerricNodeXmlRepository(fileName, byteSetRepository, xmlFactory);

            this.ferricNodeRepository = new FerricNodeCacheRepository(xmlRepository, memoryRepository);
        }

        [WebMethod]
        public void Delete(IFerricNode ferricNode)
        {
            this.ferricNodeRepository.Delete(ferricNode);
        }

        [WebMethod]
        public Guid Exists(IFerricNode ferricNode)
        {
            return this.ferricNodeRepository.Exists(ferricNode);
        }

        [WebMethod]
        public IFerricNode Find(Guid id)
        {
            return this.ferricNodeRepository.Find(id);
        }

        [WebMethod]
        public void Save(IFerricNode ferricNode)
        {
            this.ferricNodeRepository.Save(ferricNode);
        }
    }
}
