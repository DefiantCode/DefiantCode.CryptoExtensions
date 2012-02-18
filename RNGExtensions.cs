using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace CryptoExtensions
{
    public static class RNGExtensions
    {
        public static byte[] GetRandomBytes(int numBytes)
        {
            var rng = new RNGCryptoServiceProvider();
            var b = new byte[numBytes];
            rng.GetBytes(b);
            return b;
        }
    }
}
