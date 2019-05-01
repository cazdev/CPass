using CPass.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

namespace CPass.ViewModel
{
    [Serializable]
    public class ViewModel
    {
        public bool AddNew = false;
        public bool removeBool = true;
        public bool wipeBool = false;
        public bool settingsBool = false;
        public bool switchLoading = false;
        public bool copyBool = false;
        public string UserPassword { get; set; } = "";
        public List<Entry> Entries = new List<Entry>();

        private static string DiskName => Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));//return C:\\

        private static readonly string FilePath = $"{DiskName}Users\\{Environment.UserName}\\AppData\\Roaming\\CPass\\data.bin";

        public static SecureString SecString = new NetworkCredential("", "TljO3NBhu088VIE36VSAmzJqfo1qSKFg48D9VzHAAythPimmy/+KRCys1pqBptpqB4WEczUNbVG645WSWy77cCgcW7v05ur+qyWzF+ZlIA/fIhdwCJG8s41oEWkT58re").SecurePassword;
        public static string pass => new NetworkCredential("", SecString).Password;

        public void AddEntry(string name, string userName, string password)
        {
            Entries.Add(new Entry(name, userName, password));
        }
        public static void Serialize(ViewModel viewModel)
        {
            if (!Directory.Exists($"{DiskName}Users\\{Environment.UserName}\\AppData\\Roaming\\CPass"))
            {
                Directory.CreateDirectory($"{DiskName}Users\\{Environment.UserName}\\AppData\\Roaming\\CPass");
            }
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, viewModel);
            byte[] array = ms.ToArray(); //serialized array

            byte[] compressed = Compress(array);
            File.WriteAllBytes(FilePath, compressed);
            File.Encrypt(FilePath);
        }
        public static ViewModel Deserialize()
        {
            if (!File.Exists(FilePath))
            {
                return new ViewModel();
            }
            File.Decrypt(FilePath);
            byte[] compressedBytes = File.ReadAllBytes(FilePath);
            byte[] decompressed = Decompress(compressedBytes);

            MemoryStream ms = new MemoryStream(decompressed);
            BinaryFormatter bf = new BinaryFormatter();

            return bf.Deserialize(ms) as ViewModel;
        }
        public static void DeleteSerialized()
        {
            File.Decrypt(FilePath);
            File.Delete(FilePath);
        }

        private static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }
        private static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
