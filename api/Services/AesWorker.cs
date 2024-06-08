using System.Security.Cryptography;
using System.Text;

namespace api.Services
{
    public class AesWorker
    {
        public static byte[] Encrypt(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKey(key, aes.KeySize / 8);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKey(key, aes.KeySize / 8);;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(data))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decryptedData = new byte[data.Length];
                    int bytesRead = cs.Read(decryptedData, 0, decryptedData.Length);
                    return decryptedData;
                }
            }
        }

        private static byte[] DeriveKey(string key, int keySize)
        {
            using (Rfc2898DeriveBytes pbkdf2 = 
                new(key, salt: Encoding.UTF8.GetBytes("salt"), iterations: 100000, hashAlgorithm: HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(keySize);
            }
        }
    }
}