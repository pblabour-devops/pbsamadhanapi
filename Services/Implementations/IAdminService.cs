using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IAdminService
    {
        Task<GenericResponseTemplateModel<StatusManagerResponseParmsViewModel>> SearchApplicationByIPin(StatusManagerRequestParmsViewModel requestData);
        Task<GenericResponseTemplateModel<string>> ShareStaus(Int64 AppId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType);
        Task<GenericResponseTemplateModel<bool>> UnlockedApplication(Int64 AppId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType);
        Task<GenericResponseTemplateModel<GetAdminDashboardDetailsViewModel>> SearchApplication(SearchApplicationParmsViewModel requestData);
        Task<GenericResponseTemplateModel<List<AllTransactionsByAppRefIdViewModel>>> SearchTransactions(Int64 appRefId);
        Task<GenericResponseTemplateModel<string>> VerifyTransactionByAppRefId(Int64 appRefId, string uniquePaymentGatewayTransactionId, string paymentTreasuryType);
        Task<GenericResponseTemplateModel<bool>> UpdateApplicationActionLogs(Int64 appRefId);
        Task<GenericResponseTemplateModel<string>> RegisterEmpanelledPerson(UserArchitectAdditionalInfoMapping formModel);

        Task<GenericListModel<DepartmentOfficialDetailsViewModel>> GetDepartmentOfficialDetailsByRoleName(string roleName);

        Task<GenericListModel<DepartmentOfficialListViewModel>> GetDepartmentOfficialList(string roleName);

        Task<GenericServiceResultTemplate> UpdateOfficerTransfer(UpdateOfficerTransferViewModel updateOfficerTransfer);

        //Task<GenericServiceResultTemplate> ResetPasswordbyAdmin(ResetPasswordViewModel resetPassword);
        Task<GenericServiceResultTemplate> UpdateEmpanelledPersonProfileDetails(UpdateEmpanelledPersonProfileDetailsViewModel requestData);
        Task<GenericServiceResultTemplate> UpdateOfficerProfileDetails(UpdateOfficerProfileDetailsViewModel requestData);

        Task<GenericResponseTemplateModel<List<AppPaymentPart>>> SearchAppPaymentPartByAppRefId(Int64 appRefId);
        Task<GenericServiceResultTemplate> UnlockedRTBFeeDetails(Int64 appRefId);
    }
}
