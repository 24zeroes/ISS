using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sodium;

namespace EncryptionProvider
{

    public class EncryptionCore
    {
        private List<Container> _containers;
        
        
        private Dictionary<string, string> IdContainerMap;
        private void Init(string InputFolder)
        {
            _containers = new List<Container>();
            
            IdContainerMap = new Dictionary<string, string>();

            FileInfo[] InputFiles;
            try
            {
                InputFiles = new DirectoryInfo(InputFolder).GetFiles("*.json");
            }
            catch (System.Exception)
            {
                
                throw new Exception($"Folder {InputFolder} doesnt exist");
            }

            foreach (var item in InputFiles)
            {
                _containers.Add(new Container
                {
                    Keys = new KeyPair(SecretBox.GenerateNonce(), SecretBox.GenerateKey()),
                    Name = item.Name,
                    InputFullPath = item.FullName
                });
            }
        }
        private void EncryptContainers(string OutputFolder, bool RemoveOriginalFiles)
        {
            
            foreach (var item in _containers)
            {
                byte[] Info = File.ReadAllBytes(item.InputFullPath);
                JObject temp = JObject.Parse(File.ReadAllText(item.InputFullPath));

                foreach (var obj in temp)
                {
                    IdContainerMap.Add(obj.Key, item.Name);
                }

                var ciphertext = SecretBox.Create(Info, item.Keys.PublicKey, item.Keys.PrivateKey);
                File.WriteAllBytes(OutputFolder + item.Name, ciphertext);
                item.OutputPath = OutputFolder + item.Name;
                if ((ciphertext.Length != 0)&&(RemoveOriginalFiles))
                {
                    File.Delete(item.InputFullPath);
                }
            }
        }
        public EncryptionCore(string InputFolder, string OutputFolder, bool RemoveOriginalFiles)
        {
            Init(InputFolder);

            EncryptContainers(OutputFolder, RemoveOriginalFiles);
        }


        private byte[] DecryptContainer(string ContainerName)
        {
            var container = _containers.FirstOrDefault(c => c.Name == ContainerName);
            if (container != null)
            {
                byte[] cyphered = File.ReadAllBytes(container.OutputPath);
                return SecretBox.Open(cyphered, container.Keys.PublicKey, container.Keys.PrivateKey);
            }
            else
            {
                return null;
            }
        }

        public JToken GetEncryptedItem(string ItemName)
        {
            var ContainerName = IdContainerMap.FirstOrDefault(i => i.Key == ItemName);
            if (ContainerName.Value == null)
                throw new Exception($"Cannot find item {ItemName} in config.");
            string DecryptedInfo = Encoding.UTF8.GetString(DecryptContainer(ContainerName.Value));
            JObject config = JObject.Parse(DecryptedInfo);
            foreach (var item in config)
            {
                if (item.Key == ItemName)
                    return item.Value;
            }
            return null;
        }
    }
}
