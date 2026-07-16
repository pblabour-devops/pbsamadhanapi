using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IGolferRegistrationService
    {
        Task<GenericResponseTemplateModel<bool>> CheckMobileNo(string mobileNo);
        Task<GenericResponseTemplateModel<string>> AddGolferRegistrationDetails(GolferRegistration formModel);
    }
}
