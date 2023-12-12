using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ENEnueva
{
    internal class Encryp
    {
        public static string EncryptString(string plainText)
        {
            string keySecret = GetKeySecretFromConfig();
            byte[] key = Encoding.ASCII.GetBytes(keySecret);
            byte[] iv = Encoding.ASCII.GetBytes(keySecret);

            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            string keySecret = GetKeySecretFromConfig();
            byte[] key = Encoding.ASCII.GetBytes(keySecret);
            byte[] iv = Encoding.ASCII.GetBytes(keySecret);
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static string GetKeySecretFromConfig()
        {
            // Asegúrate de almacenar la clave secreta en un lugar seguro y de mantener su acceso controlado.
            // Puedes buscar información sobre Azure Key Vault para almacenar la clave secreta de manera segura.
            // Esto es solo un ejemplo.
            return "tu clave secreta";
        }
    }
}
