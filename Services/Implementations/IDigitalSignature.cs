using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IDigitalSignatureService
    {
        Task<bool> Regiater(string digitalSignatureCertificateData, User user);
        Task<string> Get_UidPid(User user);
        Task<bool> Verify(User user, string clientSignatureInfo);
    }
}
