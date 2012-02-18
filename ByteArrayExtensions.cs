using System;
using System.Linq;
using System.Text;

namespace CryptoExtensions
{
    public static class ByteArrayExtensions
    {
        public static byte[] GetBytes(this string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }

        public static byte[] GetDefaultBytes(this string text)
        {
            return GetBytes(text, Encoding.Default);
        }

        public static byte[] GetUTF8Bytes(this string text)
        {
            return GetBytes(text, Encoding.UTF8);
        }

        public static byte[] GetASCIIBytes(this string text)
        {
            return GetBytes(text, Encoding.ASCII);
        }

        public static string AsBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] FromBase64(this string text)
        {
            return Convert.FromBase64String(text);
        }

        public static string GetString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        public static string GetDefaultString(this byte[] bytes)
        {
            return GetString(bytes, Encoding.Default);
        }

        public static string GetUTF8String(this byte[] bytes)
        {
            return GetString(bytes, Encoding.UTF8);
        }

        public static string GetASCIIString(this byte[] bytes)
        {
            return GetString(bytes, Encoding.ASCII);
        }
    }
}
