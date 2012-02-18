using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CryptoExtensions
{
    public enum CryptoType
    {
        Encrypt,
        Decrypt
    }

    public static class AESExtensions
    {
        public static byte[] WriteCryptoStream(this byte[] data, byte[] key, CryptoType cryptoType)
        {
            byte[] iv = null;
            if (cryptoType == CryptoType.Decrypt)
                iv = ReadIV(data);

            return WriteCryptoStream(data, key, cryptoType, ref iv);
        }

        public static byte[] WriteCryptoStream(this byte[] data, byte[] key, CryptoType cryptoType, ref byte[] iv)
        {
            using (var aes = new RijndaelManaged())
            {
                aes.Key = key;
                if (iv != null)
                    aes.IV = iv;
                else
                    iv = aes.IV;

                ICryptoTransform crypto;

                if (cryptoType == CryptoType.Decrypt)
                    crypto = aes.CreateDecryptor();
                else
                    crypto = aes.CreateEncryptor();

                using (var ms = new MemoryStream())
                {
                    using (CryptoStream csCrypt = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
                    {
                        csCrypt.Write(data, 0, data.Length);
                        csCrypt.FlushFinalBlock();
                        return ms.ToArray();
                    }
                }
            }
        }

        public static byte[] GetKeyFromPassphrase(this string passphrase, int keySize, string passwordSalt = "AD0063DC90EF1352")
        {
            var saltBytes = Encoding.ASCII.GetBytes(passwordSalt);
            var pdb = new Rfc2898DeriveBytes(passphrase, saltBytes);
            return pdb.GetBytes(keySize);
        }

        /// <summary>
        /// Encrypts bytes with random IV as first 16 bytes
        /// </summary>
        /// <param name="data">bytes to encrypt</param>
        /// <param name="key">key used to encrypt</param>
        /// <returns></returns>
        public static byte[] Encrypt(this byte[] data, byte[] key)
        {
            byte[] iv = null;
            var eb = Encrypt(data, key, ref iv);
            var ebWithIv = new byte[eb.Length + iv.Length];
            Buffer.BlockCopy(iv, 0, ebWithIv, 0, iv.Length);
            Buffer.BlockCopy(eb, 0, ebWithIv, 16, eb.Length);
            return ebWithIv;
        }

        /// <summary>
        /// Encrypts bytes
        /// </summary>
        /// <param name="data">bytes to encrypt</param>
        /// <param name="key">key used to encrypt</param>
        /// <param name="iv">iv used to encrypt--setting this to null with use a random iv</param>
        /// <returns></returns>
        public static byte[] Encrypt(this byte[] data, byte[] key, ref byte[] iv)
        {
            return WriteCryptoStream(data, key, CryptoType.Encrypt, ref iv);
        }

        /// <summary>
        /// Decrypts bytes using the first 16 bytes of data as the iv
        /// </summary>
        /// <param name="data">bytes to encrypt</param>
        /// <param name="key">key used to encrypt</param>
        /// <returns></returns>
        public static byte[] Decrypt(this byte[] data, byte[] key)
        {
            byte[] iv = data.ReadIV();
            var eb = new byte[data.Length - iv.Length];
            Buffer.BlockCopy(data, 16, eb, 0, eb.Length);
            return Decrypt(eb, key, iv);
        }

        public static byte[] Decrypt(this byte[] data, byte[] key, byte[] iv)
        {
            return data.WriteCryptoStream(key, CryptoType.Decrypt, ref iv);
        }

        public static byte[] ReadIV(this byte[] data)
        {
            var iv = new byte[16];
            Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
            return iv;
        }

        public static byte[] GetKeyFromString(this string keyString, int keySizeInBits)
        {
            var keySizeInBytes = keySizeInBits / 8;
            var kb = Encoding.UTF8.GetBytes(keyString);
            if (kb.Length == keySizeInBytes)
                return kb;

            if (kb.Length < keySizeInBytes)
                throw new ArgumentOutOfRangeException("keyString", "keyString is too short for specific keySizeInBits");

            var newKeyBytes = new byte[keySizeInBytes];
            Buffer.BlockCopy(kb, 0, newKeyBytes, 0, keySizeInBytes);
            return newKeyBytes;
        }
    }
}
