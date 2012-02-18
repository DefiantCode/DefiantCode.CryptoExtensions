using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CryptoExtensions
{
    public static class RSAExtensions
    {
        public static byte[] Encrypt(this byte[] b, RSAParameters rsaParams)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                return rsa.Encrypt(b, true);
            }
        }

        public static string EncryptAndEncode(this byte[] b, string containerName)
        {
            return Convert.ToBase64String(b.Encrypt(ExportRSAKey(containerName)));
        }

        public static byte[] Decrypt(this byte[] b, RSAParameters rsaParams)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                return rsa.Decrypt(b, true);
            }
        }

        public static byte[] DecodeAndDecrypt(this string data, string containerName)
        {
            return Convert.FromBase64String(data).Decrypt(ExportRSAKey(containerName));
        }

        public static RSAParameters ExportRSAKey(this string containerName)
        {
            var csp = new CspParameters();
            csp.KeyContainerName = containerName;
            using (var rsa = new RSACryptoServiceProvider(csp))
            {
                return rsa.ExportParameters(true);
            }
        }

        public static RSAParameters ExportRSAKeyFromXml(this string rsaKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(File.ReadAllText(rsaKeyPath));
                return rsa.ExportParameters(true);
            }
        }

        public static string ExportRSAKeyAsXml()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                return rsa.ToXmlString(true);
            }
        }
    }
}
