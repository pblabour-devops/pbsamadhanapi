using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pbsamadhannetcoreapi.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILWBCommunicationService
    {
        Task<GenericResponseTemplateModel<string>> GetAuthToken(string rawToken);
        Task<GenericResponseTemplateModel<List<LWBApplicationDetailsViewModel>>> GetApplicationDetails(string licenceNumber, string authToken);

        Task<GenericServiceResultTemplate> ErrorLogsParams(ErrorLogParamsViewModel formModel);
        Task<GenericResponseTemplateModel<string>> GetRedirectTokenWelfareBoard(string userId, string licenceNumber, int pwbCessCollectionId, int type);

        Task<GenericServiceResultTemplate> InsertLWBMaster(LWBFormRequestViewModel formModel, IFormFileCollection files);
        Task<GenericFormModel<LWB_FundMaster>> GetLWBDetails(long id);

        Task<GenericFormModel<List<LWB_Employees_Fund>>> GetLWBEmployeeDetails(long id);
        Task<GenericFormModel<List<LWB_Employees_UnpaidWages>>> GetUnpaidWagesErrorEmployeeDetails(long id);
        Task<GenericFormModel<List<LWB_LIdAndUnpaidWages>>> GetLWBUnpaidWagesPaymentEmployeeDetails(Int64 fundMasterRefId);
        Task<GenericFormModel<int>> VerifySlotAndLicenceNumber(string financialYear, string timeSlot, string licenceNumber);

    }
}
