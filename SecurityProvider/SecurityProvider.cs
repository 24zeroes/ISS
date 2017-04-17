using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptionProvider;
using Newtonsoft.Json.Linq;
using Sodium;

namespace SecurityProvider
{
    public class SecurityCore
    {
        private List<Service> RegisteredServices;
        private KeyPair SecurityCoreKeyPair;
        protected byte[] nonce;
        protected byte[] key;
        private EncryptionCore EncCore;

        protected Dictionary<string, string> ServiceKeyPair;
        protected List<string> ContainersPath;

        public SecurityCore(string InputFolder, string OutputFolder, bool RemoveOriginalFiles)
        {
            SecurityCoreKeyPair = PublicKeyBox.GenerateKeyPair();
            RegisteredServices = new List<Service>();
            EncCore = new EncryptionCore(InputFolder, OutputFolder, RemoveOriginalFiles);
        }

        public KeyPair RegisterService(string ServiceName, List<JToken>Items,  byte[] ServicePublicKey)
        {
            if (ServicePublicKey.Length != 32)
            {
                throw new Exception("Cannot register service with such Public Key");
            }
            
            var Nonce = PublicKeyBox.GenerateNonce();

            RegisteredServices.Add(new Service
            {
                Name = ServiceName,
                PublicKey = ServicePublicKey,
                Nonce = Nonce,
                Items = Items

            });
            return new KeyPair(Nonce, SecurityCoreKeyPair.PublicKey);
        }


        protected string Container;
        protected string Authentification(string ServiceName)
        {
            var item = RegisteredServices.FirstOrDefault(s => s.Name == ServiceName);
            if (item == null)
                throw new Exception("Cannot verify service. Acess denied.");
            return "";
        }


        public JToken GetProtectedInfo(string ServiceName, string ServiceKey)
        {
            string AuthResult = Authentification(ServiceName);
            if (AuthResult == "")
            {
                //var item = RegisteredServices.FirstOrDefault(s => s.Name == ServiceName);
                //var Item = PublicKeyBox.Open(ServiceKey, item.Nonce, SecurityCoreKeyPair.PrivateKey, item.PublicKey);
                return EncCore.GetEncryptedItem(ServiceKey);
            }
            else
            {
                return null;
            }
        }

    }
}