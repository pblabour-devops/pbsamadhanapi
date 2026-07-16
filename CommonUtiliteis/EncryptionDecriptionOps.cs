using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class AES256
    {
        public static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }

        public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }


        // Encrypts plaintext using AES 128bit key and a Chain Block Cipher and returns a base64 encoded string

        public static String Encrypt(String plainText, String key)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(key)));
        }


        public static String Decrypt(String encryptedText, String key)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(key)));
        }
    }

    public static class RijndaelManagedCryptoHandler
    {
        public static string EncryptFromPlainTextToBase64(string plainText, string keyText, string ivText)
        {
            using (RijndaelManaged rjm = new RijndaelManaged
            {
                KeySize = 128,
                BlockSize = 128,
                Key = ASCIIEncoding.ASCII.GetBytes(keyText),
                IV = ASCIIEncoding.ASCII.GetBytes(ivText)
            })
            {
                Byte[] input = Encoding.UTF8.GetBytes(plainText);
                Byte[] output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
                return Convert.ToBase64String(output);
            }
        }
        public static string DecryptFromPlainTextToBase64(string base64Text, string keyText, string ivText)
        {
            using (RijndaelManaged rjm = new RijndaelManaged
            {
                KeySize = 128,
                BlockSize = 128,
                Key = ASCIIEncoding.ASCII.GetBytes(keyText),
                IV = ASCIIEncoding.ASCII.GetBytes(ivText)
            })
            {
                base64Text = base64Text.Replace(' ', '+');
                var base64EncodedBytes = Convert.FromBase64String(base64Text);
                var decrypetdBytes = rjm.CreateDecryptor().TransformFinalBlock(base64EncodedBytes, 0, base64EncodedBytes.Length);
                return Encoding.UTF8.GetString(decrypetdBytes);
            }
        }
        public static string JsonToQueryString(string jsonQuery)
        {
            string str = "?";
            str += jsonQuery.Replace(":", "=").Replace("{", "").
                        Replace("}", "").Replace(",", "&").
                            Replace("\"", "");
            return str;
        }

    }
    public static class PaymentGatewayEncDecryOps
    {
        public static string GenerateChecksum_IFMS(string plainText, string checksumKey)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string hex = "";

            byte[] hashValue = new HMACSHA512(encoder.GetBytes(checksumKey)).ComputeHash(encoder.GetBytes(plainText));
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex.ToLower();
        }
        public static string Encrypt_IFMS(string planText, string key, string iv)
        {
            int _Keysize = 128;
            RijndaelManaged rijndaelCipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = _Keysize,
                BlockSize = _Keysize,
            };
            rijndaelCipher.Key = Encoding.UTF8.GetBytes(key);
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = System.Text.Encoding.UTF8.GetBytes(planText);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public static string Decrypt_IFMS(string textToDecrypt, string key, string iv)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128
            };

            Byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            rijndaelCipher.Key = Encoding.UTF8.GetBytes(key);
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
            Byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            string a = System.Text.Encoding.UTF8.GetString(plainText);
            return a;
        }
        public static string Generatehash512_HDFC(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }

    public static class DirectLoginCryptoOps
    {
        public static string Encrypt(string textToEncrypt, string filePath)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            var _with1 = rijndaelCipher;
            _with1.Mode = CipherMode.CBC;
            _with1.Padding = PaddingMode.PKCS7;
            _with1.KeySize = 128;
            _with1.BlockSize = 128;
            byte[] pwdBytes = GetFileBytes(filePath);
            byte[] keyBytes = new byte[16];
            Int32 len = pwdBytes.Length;
            if ((len > keyBytes.Length))
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        public static string Decrypt(string textToDecrypt, string filePath)
        {
            try
            {
                textToDecrypt = textToDecrypt.Replace(" ", "+");
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                var _with1 = rijndaelCipher;
                _with1.Mode = CipherMode.CBC;
                _with1.Padding = PaddingMode.PKCS7;
                _with1.KeySize = 128;
                _with1.BlockSize = 128;
                byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
                byte[] pwdBytes = GetFileBytes(filePath);
                byte[] keyBytes = new byte[16];
                Int32 len = pwdBytes.Length;
                if ((len > keyBytes.Length))
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return filePath;
            }
        }

        private static byte[] GetFileBytes(string filepath)
        {

            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            Int32 length = Convert.ToInt32(fileStream.Length);
            byte[] buffer = new byte[length + 1];
            try
            {
                Int32 count = default(Int32);
                Int32 sum = 0;
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum = sum + count;
                }
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }

    public static class MPRRedirectEncry
    {
        public static string Encrypt(string text,string key)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string hex = "";

            byte[] hashValue = new HMACSHA256(encoder.GetBytes(key)).ComputeHash(encoder.GetBytes(text));
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return text + "|" + hex.ToLower();                                                                      
        }


        public static string EncryptingURL(string plainText, string key)
        {
            byte[] EncryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            EncryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByte = Encoding.UTF8.GetBytes(plainText);
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(EncryptKey, IV), CryptoStreamMode.Write);
            cStream.Write(inputByte, 0, inputByte.Length);
            cStream.FlushFinalBlock();
            var enc = Convert.ToBase64String(mStream.ToArray());
            return Convert.ToBase64String(mStream.ToArray());
        }
    }

    public static class PartnerPortalEncDecryOps
    {
        public static string Decrypt(string encryptedText, string key)
        {
            byte[] DecryptKey = { };
            byte[] IV = { 55, 34, 87, 64, 87, 195, 54, 21 };
            byte[] inputByte = new byte[encryptedText.Length];

            DecryptKey = System.Text.Encoding.UTF8.GetBytes(key.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            string sanitizedBase64 = encryptedText.Replace(" ", "+");
            inputByte = Convert.FromBase64String(sanitizedBase64.Trim());
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(DecryptKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByte, 0, inputByte.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            string enc = encoding.GetString(ms.ToArray());
            return encoding.GetString(ms.ToArray());
        }
    }

}
