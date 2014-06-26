using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Xml;

using DistributedCipher.Framework;

namespace DistributedCipher.ByteSetRepository.Xml
{
    public class ByteSetXmlRepository : IByteSetRepository
    {
        protected IByteSetFactory byteSetFactory;
        protected string fullFileName;
        protected string path;
        protected string xmlFileName;
        protected IXmlFactory xmlFactory;

        public ByteSetXmlRepository(string fileName, IByteSetFactory byteSetFactory, IXmlFactory xmlFactory)
            : this(null, fileName, byteSetFactory, xmlFactory)
        {
        }

        public ByteSetXmlRepository(string path, string xmlFileName, IByteSetFactory byteSetFactory, IXmlFactory xmlFactory)
        {
            this.byteSetFactory = byteSetFactory;
            this.xmlFileName = xmlFileName;
            this.path = path;
            this.xmlFactory = xmlFactory;

            if (!this.xmlFileName.EndsWith(".xml"))
            {
                if (!this.xmlFileName.EndsWith("."))
                    this.xmlFileName += ".";

                this.xmlFileName += "xml";
            }

            if (this.path == null)
                this.fullFileName = this.xmlFileName;
            else
                this.fullFileName = Path.Combine(this.path, this.xmlFileName);
        }

        public void Delete(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            Dictionary<Guid, IByteSet> byteSets = LoadFromFile();

            if (!byteSets.ContainsKey(byteSet.ID))
                throw new KeyNotFoundException(byteSet.ID.ToString());

            byteSets.Remove(byteSet.ID);

            SaveToFile(byteSets);
        }

        public Guid Exists(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            Dictionary<Guid, IByteSet> byteSets = LoadFromFile();

            return Exists(byteSets, byteSet);
        }

        public IByteSet Find(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID is empty.");

            Dictionary<Guid, IByteSet> byteSets = LoadFromFile();

            if (byteSets.ContainsKey(id))
                return byteSets[id];

            return null;
        }

        public void Save(IByteSet byteSet)
        {
            if (byteSet == null)
                throw new ArgumentNullException("Byte Set");

            Dictionary<Guid, IByteSet> byteSets = LoadFromFile();

            if (Exists(byteSets, byteSet) != Guid.Empty)
                throw new DuplicateKeyException(byteSet.ID);

            byteSets[byteSet.ID] = byteSet;

            SaveToFile(byteSets);
        }

        protected Guid Exists(Dictionary<Guid, IByteSet> byteSets, IByteSet byteSet)
        {
            if (byteSets.ContainsKey(byteSet.ID))
                return byteSet.ID;

            foreach (Guid key in byteSets.Keys)
            {
                IByteSet storedByteSet = byteSets[key];

                if (storedByteSet is IByteIndex && byteSet is IByteIndex)
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
                else if (storedByteSet is IByteMap && byteSet is IByteMap)
                {
                    IByteMap byteMap = (IByteMap)byteSet;
                    IByteMap storedByteMap = (IByteMap)storedByteSet;

                    bool isMatching = true;
                    if (byteMap.Data.Count == storedByteMap.Data.Count)
                    {
                        foreach (byte byteKey in byteMap.Data.Keys)
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

        protected Dictionary<Guid, IByteSet> LoadFromFile()
        {
            FileInfo xmlFile = new FileInfo(this.fullFileName);
            if (!xmlFile.Exists)
                return new Dictionary<Guid, IByteSet>();

            XmlDocument xmlDocument = this.xmlFactory.GenerateXmlDocument();
            xmlDocument.Load(this.fullFileName);

            return GenerateByteSets(xmlDocument.DocumentElement);
        }

        protected void SaveToFile(Dictionary<Guid, IByteSet> byteSets)
        {
            FileInfo xmlFile = new FileInfo(this.fullFileName);
            if (!xmlFile.Directory.Exists)
                xmlFile.Directory.Create();

            XmlDocument xmlDocument = this.xmlFactory.GenerateXmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            xmlDocument.AppendChild(CreateByteSetsElement(byteSets, xmlDocument));

            xmlDocument.Save(Path.Combine(this.fullFileName));
        }

        #region XML Element Creation Functions

        private XmlNode CreateByteIndexElement(IByteIndex byteIndex, XmlDocument xmlDocument)
        {
            XmlElement byteIndexElement = xmlDocument.CreateElement("IByteIndex");
            
            XmlElement idElement = xmlDocument.CreateElement("ID");
            idElement.AppendChild(xmlDocument.CreateTextNode(byteIndex.ID.ToString()));

            XmlElement dataElement = xmlDocument.CreateElement("Data");
            dataElement.AppendChild(xmlDocument.CreateTextNode(FerricHelper.Crush(byteIndex.Data)));

            byteIndexElement.AppendChild(idElement);
            byteIndexElement.AppendChild(dataElement);

            return byteIndexElement;
        }

        private XmlNode CreateByteMapElement(IByteMap byteMap, XmlDocument xmlDocument)
        {
            XmlElement byteMapElement = xmlDocument.CreateElement("IByteMap");

            XmlElement idElement = xmlDocument.CreateElement("ID");
            idElement.AppendChild(xmlDocument.CreateTextNode(byteMap.ID.ToString()));

            XmlElement dataElement = xmlDocument.CreateElement("Data");
            dataElement.AppendChild(xmlDocument.CreateTextNode(FerricHelper.Crush(byteMap.Data)));

            byteMapElement.AppendChild(idElement);
            byteMapElement.AppendChild(dataElement);

            return byteMapElement;
        }

        private XmlNode CreateByteSetsElement(Dictionary<Guid, IByteSet> byteSets, XmlDocument xmlDocument)
        {
            XmlElement byteSetsElement = xmlDocument.CreateElement("ByteSets");

            foreach (Guid id in byteSets.Keys)
                byteSetsElement.AppendChild(CreateByteSetElement(byteSets[id], xmlDocument));

            return byteSetsElement;
        }

        private XmlNode CreateByteSetElement(IByteSet byteSet, XmlDocument xmlDocument)
        {
            if (byteSet is IByteIndex)
                return CreateByteIndexElement((IByteIndex)byteSet, xmlDocument);
            else if (byteSet is IByteMap)
                return CreateByteMapElement((IByteMap)byteSet, xmlDocument);
            else
                throw new InvalidOperationException("Could not determine proper implementation of provided byte set: " + byteSet.ID.ToString());
        }

        #endregion XML Element Creation Functions

        #region XML Element Reading

        protected IByteSet GenerateByteSet(XmlElement parentElement)
        {
            string data = string.Empty;
            Guid id = Guid.Empty;

            foreach (XmlElement childElement in parentElement)
            {
                if (childElement.Name == "ID")
                    id = new Guid(childElement.InnerText);
                else if (childElement.Name == "Data")
                    data = childElement.InnerText;
            }

            if (parentElement.Name == "IByteIndex")
                return this.byteSetFactory.GenerateByteIndex(id, FerricHelper.UncrushArray(data));
            else if (parentElement.Name == "IByteMap")
                return this.byteSetFactory.GenerateByteMap(id, FerricHelper.UncrushDictionary(data));
            else
                throw new InvalidOperationException("Could not determine proper implementation of identified byte set: " + parentElement.Name + " " + id.ToString());
        }

        protected Dictionary<Guid, IByteSet> GenerateByteSets(XmlElement parentElement)
        {
            Dictionary<Guid, IByteSet> byteSets = new Dictionary<Guid, IByteSet>();

            foreach (XmlElement childElement in parentElement)
            {
                IByteSet byteSet = GenerateByteSet(childElement);

                byteSets[byteSet.ID] = byteSet;
            }

            return byteSets;
        }

        #endregion XML Element Reading
    }
}
