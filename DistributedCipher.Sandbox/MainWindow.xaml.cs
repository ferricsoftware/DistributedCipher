using System;
using System.Windows;

using DistributedCipher.Common;
using DistributedCipher.Sandbox.ViewModels;
using DistributedCipher.Framework;
using DistributedCipher.ByteSetRepository.Xml;

namespace DistributedCipher.Sandbox
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IByteSetFactory byteSetFactory = new ByteSetFactory();
            string fileName = "Byte Sets.xml";
            IXmlFactory xmlFactory = new XmlFactory();

            IByteSetRepository byteSetRepository = new ByteSetXmlRepository(fileName, byteSetFactory, xmlFactory);
            DistributedCipherFactory distributedCipherFactory = new DistributedCipherFactory(byteSetRepository, xmlFactory);

            DataContext = new MainViewModel(distributedCipherFactory);
        }
    }
}
