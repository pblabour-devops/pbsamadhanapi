using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IToDoManagerService
    {
        public Task<List<ToDoApplicationActivityMaping>> GetToDoApplicationActivityMapingList(ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, ToDoActivityModeTypeEnum toDoActivityModeType, ToDoActivityCategoryTypeEnum toDoActivityCategoryType);
        public Task<bool> CreateActivityLog(ToDoActivityLog toDoActivityLog);
        public Task<Int64> InitiateApplication(InitiateApplicationParmsViewModel initiateApplicationParms, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<Int64> Master_Seed(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SeedInitiateAppAction(Int64 appRefId, User user, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType, ApplicationTypeEnum applicationType);
        public Task<bool> ShareStatusWithInvestPunjab(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType, ToDoActivityModeTypeEnum toDoActivityModeType);
        public Task<bool> UpdateCircleRefIdInProjectSite(Int64 projectSiteRefId, int projectSiteVersion, Int64 circleRefId, CircleTypeEnum circleType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> UpdateBusinessFirstRequestNativeAppIdByIPin(Int64 iPin, Int64 investPunjabAppId, Int64 nativeAppId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<GenericResponseTemplateModel<List<ToDoUserWiseActivityViewModel>>> GetUserWiseActivities();
        public Task<GenericResponseTemplateModel<List<ToDoActivityLogViewModel>>> GetActivitiesByRootActivityId(DataTableParamsViewModel dataTableParams);
        public Task<GenericResponseTemplateModel<List<ToDoActivityWiseStepViewModel>>> GetActivitiesWiseSteps(string rootActivityRefId);
        public Task<bool> RaiseTicket(string userRefId, string rootActivityRefId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, ToDoActivityCategoryTypeEnum toDoActivityCategoryType, Int64 investPunjab_Ipin, Int64 investPunjab_AppId);
        public Task<GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>> GetOpenedTicketsByUserId(string userRefId);
        public Task<GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>> GetAssignedTicketsByManpowerUserId(string manpowerUserId, ToDoTicketStatusTypeEnum toDoTicketStatusType);
        public Task<Int64> Master_Update(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> UpdateLastModifiedDateInApplication(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SetIsDirtyFlag(Int64 appRefId, bool flagValue, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SetApplicationLock(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SetApplicationLifeCycle(Int64 appRefId, ApplicationLifeCycleStatusTypeEnum applicationLifeCycleStatusType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SendApplicationToLandingOfficer(Int64 appRefId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SendApplicationToLandingOfficerAfterObjectionResolved(Int64 appRefId, AppActionTypeEnum appActionType, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<GenericResponseTemplateModel<List<ToDoApplicationActivityMaping>>> GetActivityMappingList();

        public Task<ApplicationActionViewModel> ADD_PAYMENT_RAISED_FEE(object requestData, Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> INCREASE_PAYMENT_BATCH_COUNTER(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SET_ACTION_LOG_ID(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> UPDATE_PAYMENT_RAISED_FEE(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);

        public Task<bool> SET_ISHAVING_EMPLOYEE_FLAG(Int64 shopLicenceId, int isHavingEmployee, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<GenericResponseTemplateModel<Application>> GetApplication(Int64 appRefId);
        public Task<bool> SET_REG_REN_AMD_DATES(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SET_AMENDMENT_HISTORY_COUNTER(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> PREPARE_DATA_AS_PER_TEMP_REG_FLAG(Int64 appRefId, Int64 entityKeyId, string tempRegistrationNumber, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> PREPARE_AND_SET_AMENDMENT_HISTORY_DATA(object requestData, Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SET_IS_FEE_APPLICABLE_FLAG(Int64 appRefId, bool isFeeApplicable, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SEND_NOTIFICATION(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> WITHDRAW_APPLICATION(Int64 appRefId, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<bool> SEED_TIME_LINE_FLOW_DATA(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType);
        public Task<Int64> GET_ALCCircleId_By_LabourCircleId(Int64 circleRefId);
    }
}
