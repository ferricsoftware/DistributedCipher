using DistributedCipher.ByteSetRepository.Memory;
using DistributedCipher.ByteSetRepository.Xml;
using DistributedCipher.Common;
using DistributedCipher.Framework;
using System;
using System.Web;

namespace DistributedCipher.WebServices.Server
{
    public class Global : HttpApplication
    {
        protected static IByteSetRepository byteSetRepository;

        public static IByteSetRepository ByteSetRepository { get { return Global.byteSetRepository; } }

        protected void Application_Start(object sender, EventArgs e)
        {
            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = Application["XmlRepositoryFileName"].ToString();
            IXmlFactory xmlFactory = new XmlFactory();

            Global.byteSetRepository = new ByteSetMemoryRepository();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}