
using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace pbsamadhannetcoreapi.CommonUtiliteis.RSA
{
    public static class RsaHelper
    {
        //private static readonly RSACryptoServiceProvider _privateKey = GetPrivateKeyFromPemFile(@".\CommonUtiliteis\RSA\keys\Version_1_pri.key.pem");
        //private static readonly RSACryptoServiceProvider _publicKey = GetPublicKeyFromPemFile(@".\CommonUtiliteis\RSA\keys\Version_1_pub.key.pem");
        public static string Encrypt(string text, string publicKeyPath)
        {
            RSACryptoServiceProvider _publicKey = GetPublicKeyFromPemFile(publicKeyPath);
            var encryptedBytes = _publicKey.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encrypted, string privateKeyPath)
        {
            RSACryptoServiceProvider _privateKey = GetPrivateKeyFromPemFile(privateKeyPath);
            var decryptedBytes = _privateKey.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
        }

        private static RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        private static RSACryptoServiceProvider GetPublicKeyFromPemFile(String filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }
    }
}
