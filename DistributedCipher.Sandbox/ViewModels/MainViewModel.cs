using System;
using System.ComponentModel;
using System.Windows.Input;

using DistributedCipher.Framework;
using Microsoft.Win32;

namespace DistributedCipher.Sandbox.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        protected String configurationPath;
        protected ICommand decryptCommand;
        protected IFerricNode ferricNode;
        protected IDistributedCipherFactory distributedCipherFactory;
        protected ICommand encryptCommand;
        protected String encryptedText;
        protected ICommand loadConfigurationCommand;
        protected String nonEncryptedText;

        public ICommand DecryptCommand { get { return this.decryptCommand; } }
        public ICommand EncryptCommand { get { return this.encryptCommand; } }
        public String EncryptedText { get { return this.encryptedText; } set { if (this.encryptedText == value) return; this.encryptedText = value; OnPropertyChanged("EncryptedText"); } }
        public ICommand ExitCommand { get { return ApplicationCommands.Close; } }
        public ICommand LoadConfigurationCommand { get { return this.loadConfigurationCommand; } }
        public String NonEncryptedText { get { return this.nonEncryptedText; } set { if (this.nonEncryptedText == value) return; this.nonEncryptedText = value; OnPropertyChanged("NonEncryptedText"); } }

        public MainViewModel(IDistributedCipherFactory distributedCipherFactory)
        {
            //this.ferricNode = null;
            //this.distributedCipherFactory = distributedCipherFactory;

            //this.decryptCommand = new RelayCommand(param =>
            //{
            //    if (this.ferricNode != null)
            //        NonEncryptedText = this.ferricNode.Decrypt(EncryptedText);
            //    else
            //        NonEncryptedText = EncryptedText;
            //});

            //this.encryptCommand = new RelayCommand(param =>
            //{
            //    if (this.ferricNode != null)
            //        EncryptedText = this.ferricNode.Encrypt(NonEncryptedText);
            //    else
            //        EncryptedText = NonEncryptedText;
            //});

            //this.loadConfigurationCommand = new RelayCommand(param => 
            //{
            //    OpenFileDialog openFileDialog = new OpenFileDialog();
            //    openFileDialog.DefaultExt = ".xml";
            //    openFileDialog.InitialDirectory = this.configurationPath;

            //    if (openFileDialog.ShowDialog() == true)
            //    {
            //        int index = openFileDialog.FileName.LastIndexOf(@"\");

            //        string path = openFileDialog.FileName.Substring(0, index);
            //        string fileName = openFileDialog.FileName.Substring(index + 1, openFileDialog.FileName.Length - index - 1);

            //        IDistributedCipherRepository repository = this.distributedCipherFactory.GenerateDistributedCipherRepository(path, fileName);

            //        this.ferricNode = repository.Load();
            //    }
            //});
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
