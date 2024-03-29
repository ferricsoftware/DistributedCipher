﻿using System;
using System.ComponentModel;
using System.Web.Services;

using DistributedCipher.ByteSetRepository.Cache;
using DistributedCipher.ByteSetRepository.Memory;
using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;

namespace DistributedCipher.WebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ByteSets : WebService, IByteSetRepository
    {
        protected IByteSetRepository byteSetRepository;

        public ByteSets()
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = Application["XmlRepositoryFileName"].ToString();
            IXmlFactory xmlFactory = new XmlFactory();

            IByteSetRepository memoryRepository = new ByteSetMemoryRepository();
            IByteSetRepository xmlRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);

            this.byteSetRepository = new ByteSetCacheRepository(xmlRepository, memoryRepository);
        }

        [WebMethod]
        public void Delete(IByteSet byteSet)
        {
            this.byteSetRepository.Delete(byteSet);
        }

        [WebMethod]
        public Guid Exists(IByteSet byteSet)
        {
            return this.byteSetRepository.Exists(byteSet);
        }

        [WebMethod]
        public IByteSet Find(Guid id)
        {
            return this.byteSetRepository.Find(id);
        }

        [WebMethod]
        public void Save(IByteSet byteSet)
        {
            this.byteSetRepository.Save(byteSet);
        }
    }
}
