using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IPdfOprations
    {
       public Task<GenericFormModel<Application>> GenerateCertificate(Int64 AppId, ApplicationTypeEnum applicationType);
    }
}
