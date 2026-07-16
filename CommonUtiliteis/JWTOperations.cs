using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis.RSA;
using pbsamadhannetcoreapi.Controllers;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class JWTOperations
    {
        public static string PrepareTokenForPartnerPortal(List<TokenClaimPairViewModel> claimPairs, string publicKeyPath, string handshakeCode, string signingSecretKey, string sharedIssuerCode, string sharedAudienceCode)
        {
            var secrateKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingSecretKey));
            var signingCredentials = new SigningCredentials(secrateKey, SecurityAlgorithms.HmacSha256);

            var encryptionKey = Guid.NewGuid().ToString().Substring(0, 16);
            var iVKey = Guid.NewGuid().ToString().Substring(0, 16);
            var encryptedClaimsInfo = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(JsonConvert.SerializeObject(claimPairs), encryptionKey, iVKey);
            var encryptedHandshakeCode = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(handshakeCode, encryptionKey, iVKey);
            var checksumText = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(CreateChecksum(JsonConvert.SerializeObject(claimPairs) + "|" + handshakeCode), encryptionKey, iVKey);
            var encryptedKeys = RsaHelper.Encrypt(encryptionKey + "|" + iVKey, publicKeyPath);
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("ServerSecret", encryptedClaimsInfo),
            new Claim("Checksum", checksumText),
            new Claim("HandshakeCode", encryptedHandshakeCode),
            new Claim("SignatureKeys", encryptedKeys),
            };
            var tokenOptions = new JwtSecurityToken(
                    issuer: sharedIssuerCode,
                    audience: sharedAudienceCode,
                    claims: claims,
                    expires: DateTime.Now.AddHours(5),
                    signingCredentials: signingCredentials
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public static ThirdPartyTokenVerificationRespViewModel ValidateParterPortalToken(string token, string issuer, string audience, string signingSecretKey, string privateKeyPath, string handshakeCode)
        {
            ThirdPartyTokenVerificationRespViewModel tokenVerifyResp = new ThirdPartyTokenVerificationRespViewModel()
            {
                IsValidated = false,
                TokenEncryptedKey = null,
                TokenIVKey = null,
                ViolationMsg = ""
            };
            JwtSecurityToken validatedToken;
            var isValid = JWTOperations.ValidateToken(token, issuer, audience, signingSecretKey, out validatedToken);
            if (isValid)
            {
                string signatureKeys = validatedToken.Claims.Where(x => x.Type == "SignatureKeys").FirstOrDefault().Value;
                if (signatureKeys == null)
                {
                    tokenVerifyResp.ViolationMsg = "No signature keys provided..!";
                    isValid = false;
                }
                else
                {
                    string keyPairs = "";
                    try
                    {
                        keyPairs = RsaHelper.Decrypt(signatureKeys, privateKeyPath);
                        string providedHandshakeCode = validatedToken.Claims.Where(x => x.Type == "HandshakeCode").FirstOrDefault().Value;

                        if (providedHandshakeCode == null)
                        {
                            tokenVerifyResp.ViolationMsg = "No handshake code provided..!";
                            isValid = false;
                        }
                        else
                        {
                            providedHandshakeCode = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(providedHandshakeCode, keyPairs.Split("|")[0], keyPairs.Split("|")[1]);
                            if (providedHandshakeCode != handshakeCode)
                            {
                                tokenVerifyResp.ViolationMsg = "Invalid handshake code provided..!";
                                isValid = false;
                            }
                            else
                            {
                                string serverSecret = validatedToken.Claims.Where(x => x.Type == "ServerSecret").FirstOrDefault().Value;
                                if (serverSecret == null)
                                {
                                    tokenVerifyResp.ViolationMsg = "ServerSecret not provided..!";
                                    isValid = false;
                                }
                                else
                                {
                                    string originalChecksum = validatedToken.Claims.Where(x => x.Type == "Checksum").FirstOrDefault().Value;

                                    if (originalChecksum == null)
                                    {
                                        tokenVerifyResp.ViolationMsg = "Checksum not provided..!";
                                        isValid = false;
                                    }
                                    else
                                    {
                                        originalChecksum = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(originalChecksum, keyPairs.Split("|")[0], keyPairs.Split("|")[1]);
                                        string strigyfyClaimPairs = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(serverSecret, keyPairs.Split("|")[0], keyPairs.Split("|")[1]);
                                        var checksumText = CreateChecksum(strigyfyClaimPairs + "|" + providedHandshakeCode);
                                        if (checksumText != originalChecksum)
                                        {
                                            tokenVerifyResp.ViolationMsg = "Invalid Checksum provided..!";
                                            isValid = false;
                                        }
                                        else
                                        {
                                            tokenVerifyResp.IsValidated = true;
                                            tokenVerifyResp.TokenEncryptedKey = keyPairs.Split("|")[0];
                                            tokenVerifyResp.TokenIVKey = keyPairs.Split("|")[1];
                                            tokenVerifyResp.ViolationMsg = "All ok";
                                        }

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        tokenVerifyResp.ViolationMsg = ex.Message;
                    }

                }
            }
            else
            {
                tokenVerifyResp.ViolationMsg = "Invalid token..!";
            }
            return tokenVerifyResp;
        }


        private static bool ValidateToken(string token, string issuer, string audience, string signingSecretKey, out JwtSecurityToken jwt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingSecretKey));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var extractedToken = tokenHandler.ReadJwtToken(token);
                if(extractedToken.Issuer == issuer && extractedToken.Audiences.Any(x=>x == audience))
                {
                    jwt = (JwtSecurityToken)extractedToken;
                    return true;
                }
                //tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                jwt = null;
                return false;
            }
            catch (SecurityTokenValidationException ex)
            {
                // Log the reason why the token is not valid
                jwt = null;
                return false;
            }
        }


        private static string CreateChecksum(string textToBeHashed)
        {
            string checksumText;
            using (var md5 = MD5.Create())
            {
                byte[] checksum = md5.ComputeHash(Encoding.UTF8.GetBytes(textToBeHashed));
                checksumText = BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
            }
            return checksumText;
        }
    }
}
