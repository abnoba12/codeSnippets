using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WE.Freamwork.Common.Helpers
{
    public static class HashingHelper
    {
        #region One way hashing
        private static int saltLengthLimit = 32;

        public static byte[] GenerateNewSalt(int? saltLimit = null)
        {
            var sl = saltLimit ?? saltLengthLimit;
            return GetSalt(sl);
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public static string GenerateSaltedHashString(byte[] plainText, byte[] salt)
        {
            return Convert.ToBase64String(GenerateSaltedHash(plainText, salt));
        }

        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static bool ComparePasswordByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }


        public static string ConvertToBase64String(this string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ConvertToByteArray(this string text)
        {
            return Convert.FromBase64String(text.ConvertToBase64String());
        }
        #endregion

        #region Two way hashing
        /// <summary>
        /// Provided text and a key, this will encrypt a string. Only the provided key will decrypt the string back to its origional
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <param name="key">At a minimum this needs to be at least 8 characters long. The longer the better though.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string Encrypt(string textToEncrypt, string key)
        {
            try
            {
                string ToReturn = "";
                string _key = key.Substring(0, 8);
                string _iv = key;

                byte[] _ivByte = { };
                _ivByte = Encoding.UTF8.GetBytes(_iv);

                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);

                MemoryStream ms = null; CryptoStream cs = null;

                byte[] inputbyteArray = Encoding.UTF8.GetBytes(System.Web.HttpUtility.UrlEncode(textToEncrypt));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }

                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

        /// <summary>
        /// Turn an encrypted string back to its origional form. The key used to encrypt it is needed.
        /// </summary>
        /// <param name="textToDecrypt"></param>
        /// <param name="key">Key used when encrypting the string</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string Decrypt(string textToDecrypt, string key)
        {
            try
            {
                string ToReturn = "";
                string _key = key.Substring(0, 8);
                string _iv = key;

                byte[] _ivByte = { };
                _ivByte = Encoding.UTF8.GetBytes(_iv);

                byte[] _keybyte = { };
                _keybyte = Encoding.UTF8.GetBytes(_key);

                MemoryStream ms = null; CryptoStream cs = null;

                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }

                return System.Web.HttpUtility.UrlDecode(ToReturn);
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

        /// <summary>
        /// Generate a random key of X lenth
        /// </summary>
        /// <param name="stringLength"></param>
        /// <returns></returns>
        public static string GenerateKey(int stringLength)
        {
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Iterates through an object and will encrypt every string field
        /// </summary>
        /// <param name="o"></param>
        public static void EncryptFieldsInObject(Object o, string key)
        {
            var properties = o.GetType()                                    // current type 
              .GetProperties(BindingFlags.Public | BindingFlags.Instance) // public, not static
              .Where(p => p.CanRead && p.CanWrite)                       // can read and write
              .Where(p => !p.GetIndexParameters().Any())                  // not indexer 
              .Where(p => p.PropertyType == typeof(string));

            foreach (PropertyInfo p in properties)
            {
                var fieldValue = p.GetValue(o)?.ToString();
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    p.SetValue(o, Encrypt(fieldValue, key), null);
                }
            }
        }

        /// <summary>
        /// Iterates through an object and will decrypt every string field
        /// </summary>
        /// <param name="o"></param>
        public static void DecryptFieldsInObject(Object o, string key)
        {
            var properties = o.GetType()                                    // current type 
              .GetProperties(BindingFlags.Public | BindingFlags.Instance) // public, not static
              .Where(p => p.CanRead && p.CanWrite)                       // can read and write
              .Where(p => !p.GetIndexParameters().Any())                  // not indexer 
              .Where(p => p.PropertyType == typeof(string));

            foreach (PropertyInfo p in properties)
            {
                var fieldValue = p.GetValue(o)?.ToString();
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    p.SetValue(o, Decrypt(fieldValue, key), null);
                }
            }
        }
        #endregion
    }
}
