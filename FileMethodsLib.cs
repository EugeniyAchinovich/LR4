using System;
using System.ServiceProcess;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace FileMethods
{
	public class FileMethods
	{
		
		public string SetArchiveName(string filePath)
        {
            string name = File.GetCreationTime(filePath).ToString();

            name = name.Replace(".", "_");
            name = name.Replace(":", "_");
            if (!archiveDateOnly)
                name += " " + Path.GetFileNameWithoutExtension(filePath);

            return name;
        }

        public void CompressFile(string sourceFile, string compressedFile)
        {
            lock (obj)
            {
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(compressedFile))
                    {
                        using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                        {
                            sourceStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
        }

        public void DecompressFile(string compressedFile, string targetFile)
        {
            lock (obj)
            {
                using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
                {
                    using (FileStream targetStream = File.Create(targetFile))
                    {
                        using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                        {
                            decompressionStream.CopyTo(targetStream);
                        }
                    }
                }
            }
        }

        public byte[] EncryptStringToBytes_Aes(string Text, byte[] Key, byte[] IV)
        {
            if (Text == null || Text.Length <= 0)
                throw new ArgumentNullException("The file is empty");

            byte[] encryptedByte;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Text);
                        }
                        encryptedByte = msEncrypt.ToArray();
                    }
                }
            }
            return encryptedByte;
        }

        public string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string decryptedText = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return decryptedText;
        }

        public void EncryptFile(FileSystemEventArgs e, Aes aes)
        {
            string str;
            using (StreamReader streamReader = new StreamReader(e.FullPath))
            {
                str = streamReader.ReadToEnd();
            }

            encryptingCypher = EncryptStringToBytes_Aes(str, aes.Key, aes.IV);

            using (StreamWriter streamWriter = new StreamWriter(e.FullPath))
            {
                foreach (byte b in encryptingCypher)
                {
                    streamWriter.Write(b);
                }
            }
        }

        public void DecryptFile(FileSystemEventArgs e, Aes aes, string path = null)
        {
            string str;
            if (path == null) path = e.FullPath;

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                str = DecryptStringFromBytes_Aes(encryptingCypher, aes.Key, aes.IV);
                streamWriter.Write(str);
            }
        }
		public FileMethods() { }
	}
}