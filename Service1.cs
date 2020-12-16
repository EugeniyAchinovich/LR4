using System;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml;

// пользовательские namespace
using ServiceLayer;
using DB_Table;
using XML_File_Generator;
using ConfigManager;
using FileMethodsLib;

namespace UTL_Service
{
    public partial class Service1 : ServiceBase
    {
        Logger logger;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(3500);
        }
    }

    class JsonConfig
    {
        public string sourceFolder { get; set; }
        public string targetFolder { get; set; }
        public string archive { get; set; }
        public bool archiveDateOnly { get; set; }
        public bool needToArchive { get; set; }
        public bool needToCompress { get; set; }
        public bool needToEncrypt { get; set; }
        public string cypherKey { get; set; }
    }

    class XMLConfig
    {
        public string sourceFolder { get; set; }
        public string targetFolder { get; set; }
        public string archive { get; set; }
        public bool archiveDateOnly { get; set; }
        public bool needToArchive { get; set; }
        public bool needToCompress { get; set; }
        public bool needToEncrypt { get; set; }
        public string cypherKey { get; set; }
    }

    class Logger
    {
        FileSystemWatcher watcher;
        object obj = new object();
        bool enabled = true;

		// поля по умолчанию
        string sourceFolder = @"C:\Users\Asus\Desktop\SourceFolder";
        string targetFolder = @"C:\Users\Asus\Desktop\TargetFolder";
        string archive = @"C:\Users\Asus\Desktop\TargetFolder\Archive";
        bool archiveDateOnly = false;
        bool needToArchive = true;
        bool needToCompress = true;
        bool needToEncrypt = true;
        string cypherKey = "key";
        static byte[] encryptingCypher;

        static string connectionString = "Data Source=.\\SQLDef;Initial Catalog=AdventureWorksLT2019;Integrated Security=True";

        string jsonFilePath = @"C:\Users\Asus\Desktop\config.json";
        string xmlFilePath = @"C:\Users\Asus\Desktop\config.xml";

        public Logger()
        {
            watcher = new FileSystemWatcher(sourceFolder);
            watcher.Created += MainProcess;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            while (enabled)
            {
                Thread.Sleep(3500);
            }
        }
        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        private void MainProcess(object sender, FileSystemEventArgs e)
        {
            ConfigManager configManager = new ConfigManager();
            ServiceLayer serviceLayer = new ServiceLayer();

            configManager.ManageConfigs();

            var connectionString = serviceLayer.ConnectionString;
            int id = 1;
            string archiveSubdirectory = $@"{archive}\{FileMethods.SetArchiveName(e.FullPath)}";
            string compressedPath = $@"{targetFolder}\{Path.GetFileNameWithoutExtension(e.FullPath)}.gz";
            string decompressedPath = $@"{archiveSubdirectory}\{Path.GetFileName(e.FullPath)}.xml";

            Person person = new Person(connectionString, id);
            Sales sales = new Sales(connectionString, id);
            Production production = new Production(connectionString, id);

            DB_Table db_table = new DB_Table(person, null, null);
            XMLGenerator xmlGenerator = new XmlGenerator();

            xmlGenerator.CreateXMLFile(db_table, sourceFolder);

            using (Aes myAes = Aes.Create())
            {
                if (needToArchive && needToEncrypt)
                {
                    DirectoryInfo directory = Directory.CreateDirectory(archiveSubdirectory);

                    FileMethods.EncryptFile(e, myAes);
                    FileMethods.CompressFile(e.FullPath, compressedPath);
                    FileMethods.DecompressFile(compressedPath, decompressedPath);
                    FileMethods.DecryptFile(e, myAes);
                    FileMethods.DecryptFile(e, myAes, decompressedPath);
                }
                if (!needToArchive && needToEncrypt)
                {
                    FileMethods.EncryptFile(e, myAes);
                    FileMethods.CompressFile(e.FullPath, compressedPath);
                    FileMethods.DecryptFile(e, myAes);
                }
                if (needToArchive && !needToEncrypt)
                {
                    DirectoryInfo directory = Directory.CreateDirectory(archiveSubdirectory);

                    FileMethods.CompressFile(e.FullPath, compressedPath);
                    FileMethods.DecompressFile(compressedPath, decompressedPath);
                }
                if (!needToArchive && !needToEncrypt)
                {
                    FileMethods.CompressFile(e.FullPath, compressedPath);
                }
            }
        }
    }
}