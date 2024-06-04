using System.Security.Cryptography;

namespace api.Services
{
    public class AesWorker
    {
        public static byte[] EncryptPicture(byte[] pictureBytes, byte[] encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(pictureBytes, 0, pictureBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DecryptPicture(byte[] encryptedPictureBytes, byte[] encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.GenerateIV();

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new(encryptedPictureBytes))
                {
                    using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[encryptedPictureBytes.Length];
                        int bytesRead = cs.Read(decryptedBytes, 0, decryptedBytes.Length);
                        byte[] result = new byte[bytesRead];
                        Array.Copy(decryptedBytes, 0, result, 0, bytesRead);
                        return result;
                    }
                }
            }
        }
    }
}