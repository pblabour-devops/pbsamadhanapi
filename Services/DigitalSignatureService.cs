using Newtonsoft.Json;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
//using Signer.Digital.WebLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        private readonly IGenericRepository<DigitalSignature> _iGR_DigitalSignature;
        private readonly IGenericRepository<DigitalSignature_Log> _iGR_DigitalSignature_Log;

        public DigitalSignatureService(IGenericRepository<DigitalSignature> iGR_DigitalSignature, IGenericRepository<DigitalSignature_Log> iGR_DigitalSignature_Log) 
        {
            _iGR_DigitalSignature = iGR_DigitalSignature;
            _iGR_DigitalSignature_Log = iGR_DigitalSignature_Log;
        }
        public async Task<bool> Regiater(string digitalSignatureCertificateData, User user)
        {
            DigitalSignatureCertificateAttributes digitalSignatureCertificateAttributes = JsonConvert.DeserializeObject<DigitalSignatureCertificateAttributes>("{\"SelCertSubject\": \"OID.2.5.4.65=ac59b47f54011b990b928bde9a8b8b96, T=3260, CN=Narinder Singh,O=Personal,SERIALNUMBER=d898b2c46c897e0b11ca5c20a7d65642178eaf5fc05ec7d6aa9e93673cb8432d, STREET=1397,Sector 68,S.A.S.Nagar(Mohali),Sector 62,S.A.S.Nagar(mohali),SAS Nagar(Mohali),PostalCode=160062,Phone=4db7d756461da75b7553bdb6a95843d418f35e106c837ab5b82df7c8800bfb46, S=Punjab,C=IN\",\"CertThumbPrint\":\"B8A67FA5091771CD3365CA7E713AF12CAE3E6C1D\",\"PublicKey\":\"MIIBCgKCAQEA2fHhcRPBvaRh9ZSb3MIExX/uqo9dPLYhHI3ISrqlp8o7zAOA7FeFz4NGZQAykRcFLmYPf/xTpIYG9G5cWF+ci1mupNxWarzlO2JktbgQg3EhvndA8cGi4qx0CDp8nSGMjLnAEkyYg5fFZMchQEyC2+3HlJscX4+AZnUfeI3HOcy2NwS5c4EU2QMx2Fd2M0cR5r1U4ilZwUcet/l7PGIoOUFWzaipNNXX7fRhDoJrFOe4b90u/9PYaqHDe7r05l237EQu03T8T++w5bm7tWe5sQuyunMX9sPuF8zNKkHJpLaWyuFeD6L4FD6Mgg5BgiG4L6xwKkcdTsUjMDnPGEvo/QIDAQAB\",\"Cert\":\"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\",\"eMail\": \"ddfnarinder@gmail.com\",\"ValidFrom\": \"2021-03-03T20:17:21+05:30\",\"ExpDate\": \"2023-03-03T20:17:21+05:30\"}");
            DigitalSignature digitalSignature = user.UserProfileMapping.UserProfile.DigitalSignature;

            if (digitalSignature == null)
            {
                digitalSignature = new DigitalSignature()
                {
                    JsonData = digitalSignatureCertificateData,
                    CertThumbPrint = digitalSignatureCertificateAttributes.CertThumbPrint,
                    EMail = digitalSignatureCertificateAttributes.eMail,
                    ExpiryDate = digitalSignatureCertificateAttributes.ExpDate,
                    PublicKey = digitalSignatureCertificateAttributes.PublicKey,
                    RegistrationDate = DateTime.Now,
                    SelCertSubject = digitalSignatureCertificateAttributes.SelCertSubject,
                    UserProfileRefId = user.UserProfileMapping.UserProfileRefId,
                    ValidFrom = digitalSignatureCertificateAttributes.ValidFrom,
                    VerificationPid = Guid.NewGuid().ToString(),
                    VerificationUid = Guid.NewGuid().ToString()
                };

                _iGR_DigitalSignature.Insert(digitalSignature);
                await _iGR_DigitalSignature.SavechangeAsync();
            }
            else
            {
                DigitalSignature_Log digitalSignature_Log = new DigitalSignature_Log();
                digitalSignature_Log = JsonConvert.DeserializeObject<DigitalSignature_Log>(JsonConvert.SerializeObject(digitalSignature));
                digitalSignature_Log.DigitalSignatureRefId = digitalSignature.DigitalSignatureId;

                _iGR_DigitalSignature_Log.Insert(digitalSignature_Log);
                await _iGR_DigitalSignature_Log.SavechangeAsync();

                digitalSignature.JsonData = digitalSignatureCertificateData;
                digitalSignature.CertThumbPrint = digitalSignatureCertificateAttributes.CertThumbPrint;
                digitalSignature.EMail = digitalSignatureCertificateAttributes.eMail;
                digitalSignature.ExpiryDate = digitalSignatureCertificateAttributes.ExpDate;
                digitalSignature.PublicKey = digitalSignatureCertificateAttributes.PublicKey;
                digitalSignature.RegistrationDate = DateTime.Now;
                digitalSignature.SelCertSubject = digitalSignatureCertificateAttributes.SelCertSubject;
                digitalSignature.UserProfileRefId = user.UserProfileMapping.UserProfileRefId;
                digitalSignature.ValidFrom = digitalSignatureCertificateAttributes.ValidFrom;
                digitalSignature.VerificationPid = Guid.NewGuid().ToString();
                digitalSignature.VerificationUid = Guid.NewGuid().ToString();

                _iGR_DigitalSignature.Update(digitalSignature);
                await _iGR_DigitalSignature.SavechangeAsync();
            }
            return true;
        }

        public async Task<string> Get_UidPid(User user)
        {
            DigitalSignature digitalSignature = user.UserProfileMapping.UserProfile.DigitalSignature;
            if (digitalSignature != null)
            {
                return digitalSignature.VerificationUid + "|" + digitalSignature.VerificationPid;
            }
            return null;
        }
        public async Task<bool> Verify(User user, string clientSignatureInfo)
        {
            var tt = JsonConvert.DeserializeObject(clientSignatureInfo);
           // if (SDAuthorize.VerifyAuthToken("34e049d0-de7b-41ab-9eb4-ee03366664d8|dbff3c46-3434-453e-bed2-2afe5ceb06ad", tt.ToString(), "MIIBCgKCAQEA2fHhcRPBvaRh9ZSb3MIExX/uqo9dPLYhHI3ISrqlp8o7zAOA7FeFz4NGZQAykRcFLmYPf/xTpIYG9G5cWF+ci1mupNxWarzlO2JktbgQg3EhvndA8cGi4qx0CDp8nSGMjLnAEkyYg5fFZMchQEyC2+3HlJscX4+AZnUfeI3HOcy2NwS5c4EU2QMx2Fd2M0cR5r1U4ilZwUcet/l7PGIoOUFWzaipNNXX7fRhDoJrFOe4b90u/9PYaqHDe7r05l237EQu03T8T++w5bm7tWe5sQuyunMX9sPuF8zNKkHJpLaWyuFeD6L4FD6Mgg5BgiG4L6xwKkcdTsUjMDnPGEvo/QIDAQAB"))
               // return true;
            //else
                return false;
        }
    }
}
