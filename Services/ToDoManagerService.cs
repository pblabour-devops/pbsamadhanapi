using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class ToDoManagerService : IToDoManagerService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private IConfiguration _iConfiguration { get; }
        private readonly INotificationManagerService _iNotificationManagerService;
        private IAuthService _iAuthService;
        private readonly IDapperRepository _iDapperRepository;
        private readonly IAppTimeLineManagerService _iAppTimeLineManagerService;
        public ToDoManagerService(
            AppDbContext context,
            UserManager<User> userManager,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IConfiguration iConfiguration,
            INotificationManagerService iNotificationManagerService,
            IAuthService authService,
            IDapperRepository iDapperRepository,
            IAppTimeLineManagerService iAppTimeLineManagerService
            )
        {
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _context = context;
            _userManager = userManager;
            _iConfiguration = iConfiguration;
            _iNotificationManagerService = iNotificationManagerService;
            _iAuthService = authService;
            _iDapperRepository = iDapperRepository;
            _iAppTimeLineManagerService = iAppTimeLineManagerService;
        }
        public async Task<List<ToDoApplicationActivityMaping>> GetToDoApplicationActivityMapingList(ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, ToDoActivityModeTypeEnum toDoActivityModeType, ToDoActivityCategoryTypeEnum toDoActivityCategoryType)
        {
            return await _context.ToDoApplicationActivityMapings.Where(x => x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType && x.ToDoActivityModeType == toDoActivityModeType && x.ToDoActivityCategoryType == toDoActivityCategoryType).OrderBy(x => x.ToDoSerialOrderCount).ToListAsync();
        }
        public async Task<bool> CreateActivityLog(ToDoActivityLog toDoActivityLog)
        {
            await _context.AddAsync(toDoActivityLog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Int64> InitiateApplication(InitiateApplicationParmsViewModel parms, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            Application application = new Application()
            {
                ApplicationType = parms.ApplicationType,
                ApplicationPurposeType = parms.ApplicationPurposeType,
                CreatedOnDate = DateTime.Now,
                LastModifiedOnDate = DateTime.Now,
                IsDeleted = false,
                IsEnabled = true,
                IsDigitalSignatureRequired = false,
                IsDigitalSignatureVerified = false,
                DigitalSignatureRefId = null,
                IsAllowEdit = true,
                IsLocked = false,
                ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED,
                ApplicationLifeCycleLastStatusOn = DateTime.Now,
                PaymentBatchCounter = 1,
                Legacy_AppId = parms.Legacy_AppId,
                Legacy_IsMigrated = parms.Legacy_IsMigrated,
                Legacy_AppFormId = parms.Legacy_AppFormId,
                Legacy_NAR = parms.Legacy_NAR,
                InvestPunjab_Ipin = parms.IPin.ToString(),
                InvestPunjab_AppId = parms.InvestPunjab_AppId,
                Legacy_LicenceNo = parms.Legacy_LicenceNo,
                ProjectSiteVersion = parms.ProjectSiteVersion,
                IsIPIntegrated = true,
                IsTimeLineFlow = (parms.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE
                               || parms.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP
                               || parms.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY
                               || parms.ApplicationType == ApplicationTypeEnum.OSH_FORM_1_Registration
                               || parms.ApplicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS) ? false : true
            };
            if (parms.ApplicationType == ApplicationTypeEnum.REG_ESTB_OSH || parms.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH || parms.ApplicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
            {
                application.IsFeeApplicable = true;
                application.DepartmentRefID = 2;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
            {
                application.IsFeeApplicable = true;
                application.DepartmentRefID = 3;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD || parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN || parms.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE || parms.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR || parms.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || parms.ApplicationType == ApplicationTypeEnum.TRADE_UNION || parms.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || parms.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
            {
                application.IsFeeApplicable = true;
                application.DepartmentRefID = 3;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE || parms.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP || parms.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE || parms.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 1;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
            {
                application.IsFeeApplicable = true;
                application.DepartmentRefID = 1;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 3;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 3;
            }
            else if (parms.ApplicationType == ApplicationTypeEnum.OSH_FORM_1_Registration)
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 3;
            }
            #region for samadhan
            else if (parms.ApplicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS)
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 3;
            }
            #endregion
            else
            {
                application.IsFeeApplicable = false;
                application.DepartmentRefID = 1;
            }

            if (parms.ApplicationType == ApplicationTypeEnum.MOTOR_TRANSPORT || parms.ApplicationType == ApplicationTypeEnum.TRADE_UNION)
            {
                application.IsIPIntegrated = false;
                application.IsTimeLineFlow = false;
            }

            //var user = await _userManager.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).FirstOrDefaultAsync(x => x.Id == parms.UserId);


            int alreadyItrationCount = await _context.Applications.CountAsync(x => x.ProjectSiteRefId == parms.ProjectSiteRefId
                && x.IsDeleted == false
                && x.ApplicationType == application.ApplicationType
                && x.ApplicationPurposeType == application.ApplicationPurposeType);

            application.PublicAppRefNum = "TempPublicRefNumber";
            application.ProjectSiteRefId = parms.ProjectSiteRefId;
            application.IterationCount = alreadyItrationCount + 1;
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            application = await _context.Applications.Where(x => x.AppId == application.AppId && x.IsDeleted == false).FirstOrDefaultAsync();
            if (application != null)
            {
                string appTypeAbbr = "";
                string appPurposeTypeCode = "";
                if (application.ApplicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                {
                    appTypeAbbr = "ER";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH)
                {
                    appTypeAbbr = "CL";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                {
                    appTypeAbbr = "BP";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                {
                    appTypeAbbr = "CM";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    appTypeAbbr = "HUD";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE)
                {
                    appTypeAbbr = "SC";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN)
                {
                    appTypeAbbr = "BPP";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    appTypeAbbr = "FL";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP)
                {
                    appTypeAbbr = "NTS";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                {
                    appTypeAbbr = "CL";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                {
                    appTypeAbbr = "BPS";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
                {
                    appTypeAbbr = "NTF";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                {
                    appTypeAbbr = "BOC";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                {
                    appTypeAbbr = "BOC";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                {
                    appTypeAbbr = "BPP";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING)
                {
                    appTypeAbbr = "BPE";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    appTypeAbbr = "BPA";
                }

                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    appTypeAbbr = "PSIC";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                {
                    appTypeAbbr = "PE";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.TRADE_UNION)
                {
                    appTypeAbbr = "TU";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                {
                    appTypeAbbr = "ISM";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                {
                    appTypeAbbr = "ICL";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.OSH_FORM_1_Registration)
                {
                    appTypeAbbr = "ER";
                }
                #region for samadhan
                else if (application.ApplicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS)
                {
                    appTypeAbbr = "SA";
                }
                #endregion


                appPurposeTypeCode = application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE ? "G" : application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE ? "R" : "A";

                application.PublicAppRefNum = appTypeAbbr + appPurposeTypeCode + application.IterationCount.ToString() + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + application.AppId.ToString("D8");
                _context.Entry(application).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = application.AppId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return application.AppId;
        }

        public async Task<Int64> Master_Seed(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            await _context.AddAsync(requestData);
            await _context.SaveChangesAsync();
            PropertyInfo entityKeyProp = requestData.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(KeyAttribute)));

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = requestData.GetType().Name,
                PrimaryKeyValue = Convert.ToInt64(entityKeyProp.GetValue(requestData)),
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return Convert.ToInt64(entityKeyProp.GetValue(requestData));
        }

        public async Task<bool> SeedInitiateAppAction(Int64 appRefId, User user, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType, ApplicationTypeEnum applicationType)
        {

            var userDetail = await _userManager.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).FirstOrDefaultAsync(x => x.Id == user.Id);

            string ReceiverRoleId = "6dff5abf-f8d3-4f80-9310-491b1cb2dbe2";
            var SenderRoleId = userDetail.UserRoles.FirstOrDefault().RoleId;

            if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC || applicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS)  // for samadhaan
            {
                ApplicationAction_ParallelProcess applicationAction_ParallelProcess = new ApplicationAction_ParallelProcess()
                {
                    ActionDate = DateTime.Now,
                    ActionTakenDaysCount = 0,
                    ActionTakenHoursCount = 0,
                    AppActionType = 1,
                    ApplicationRefId = appRefId,

                    Receiver_ProfileRefId = userDetail.UserProfileMapping.UserProfileRefId,
                    Receiver_UserRefId = userDetail.Id,

                    Sender_ProfileRefId = userDetail.UserProfileMapping.UserProfileRefId,
                    Sender_UserRefId = user.Id,
                    Remarks = "Application drafted",
                    ReceiverRoleId = ReceiverRoleId,
                    SenderRoleId = SenderRoleId,
                    AppDocumentRefId = 0,
                    IsDocumentUploaded = false,

                    IpAddress = "38.183.8.23",
                    Latitude = "30.703616",
                    Longitude = "76.7623168",
                    IsDeleted = false,
                    IsAlive = true
                };

                await _context.ApplicationAction_ParallelProcesses.AddAsync(applicationAction_ParallelProcess);
                await _context.SaveChangesAsync();
            }
            else
            {
                ApplicationAction applicationAction = new ApplicationAction()
                {
                    ActionDate = DateTime.Now,
                    ActionTakenDaysCount = 0,
                    ActionTakenHoursCount = 0,
                    AppActionType = 1,
                    ApplicationRefId = appRefId,

                    Receiver_ProfileRefId = userDetail.UserProfileMapping.UserProfileRefId,
                    Receiver_UserRefId = userDetail.Id,

                    Sender_ProfileRefId = userDetail.UserProfileMapping.UserProfileRefId,
                    Sender_UserRefId = user.Id,
                    Remarks = "Application drafted",
                    ReceiverRoleId = ReceiverRoleId,
                    SenderRoleId = SenderRoleId,
                    AppDocumentRefId = 0,
                    IsDocumentUploaded = false,

                    IpAddress = "38.183.8.23",
                    Latitude = "30.703616",
                    Longitude = "76.7623168"
                };

                await _context.ApplicationActions.AddAsync(applicationAction);
                await _context.SaveChangesAsync();

                ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                {
                    ActionDate = applicationAction.ActionDate,
                    ActionTakenDaysCount = 0,
                    ActionTakenHoursCount = 0,
                    AppActionType = applicationAction.AppActionType,
                    ApplicationRefId = applicationAction.ApplicationRefId,

                    Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                    Receiver_UserRefId = applicationAction.Receiver_UserRefId,

                    Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
                    Sender_UserRefId = applicationAction.Sender_UserRefId,
                    Remarks = applicationAction.Remarks,
                    ReceiverRoleId = applicationAction.ReceiverRoleId,
                    SenderRoleId = applicationAction.SenderRoleId,
                    AppDocumentRefId = 0,
                    IsDocumentUploaded = false,

                    IpAddress = applicationAction.IpAddress,
                    Latitude = applicationAction.Latitude,
                    Longitude = applicationAction.Longitude
                };

                await _context.ApplicationActionLogs.AddAsync(applicationActionlogs);
                await _context.SaveChangesAsync();
            }

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationActionLog",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }


        public async Task<bool> ShareStatusWithInvestPunjab(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType, ToDoActivityModeTypeEnum toDoActivityModeType)
        {
            List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
            {
                new StoreProcedureParm (){ ParmName="appRefId", ParmValue=appRefId.ToString(), isNumber=true}
            };
            var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetShareStatusRequestDetailsViewModel>("dbo.sp_GetShareStatusRequestDetails", storeProcedureParms1);
            AppActionTypeEnum actionType = AppActionTypeEnum.DEFAULT;

            if (toDoActivityModeType == ToDoActivityModeTypeEnum.ADD_NEW_MODE || toDoActivityModeType == ToDoActivityModeTypeEnum.EDIT_MODE)
            {
                actionType = AppActionTypeEnum.APP_SAVE_DRAFT;
            }
            else if (toDoActivityModeType == ToDoActivityModeTypeEnum.LOCK_APPLICATION_MODE)
            {
                actionType = (resp.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE
                    || resp.FirstOrDefault().ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY
                    || resp.FirstOrDefault().ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP
                    || resp.FirstOrDefault().ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
                    ? AppActionTypeEnum.APP_SUBMITTED_FEE_NOT_APPLICABLE
                    : AppActionTypeEnum.APP_SAVE_LOCK_FEE_PENDING;
            }
            else if (toDoActivityModeType == ToDoActivityModeTypeEnum.RESOLVE_OBJECTION_MODE)
            {
                actionType = AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED;
            }

            var shareStatusLog = await _context.BusinessFirstShareStatusLogs.Where(x => x.AppRefId == appRefId && x.IsRequestCompeleted == true).ToListAsync();
            if (actionType == AppActionTypeEnum.APP_SAVE_DRAFT && shareStatusLog.Count() > 0)
            {

            }
            else
            {
                if (resp != null)
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="appRefId", ParmValue=appRefId.ToString(), isNumber=true}
                    };
                    var totalTakenTimeInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetTotalTakenTimeByDepartmentViewModel>("dbo.sp_GetTotalTakenTimeByDepartment", storeProcedureParms);
                    var requestData = new
                    {
                        iPin = resp.FirstOrDefault().InvestPunjab_Ipin,
                        AppId = resp.FirstOrDefault().InvestPunjab_AppId,
                        statusId = resp.FirstOrDefault().AppActionType == 5 ? 2 : resp.FirstOrDefault().AppActionType,
                        statusDesc = resp.FirstOrDefault().Remarks,
                        comments = resp.FirstOrDefault().Remarks,
                        senderName = resp.FirstOrDefault().SenderName,
                        senderDesignation = resp.FirstOrDefault().SenderRoleName,
                        receiverName = resp.FirstOrDefault().ReceiverName,
                        receiverDesignation = resp.FirstOrDefault().ReceiverRoleName,
                        clearanceIssuedOn = resp.FirstOrDefault().AppActionType != 200 ? "NA" : resp.FirstOrDefault().ActionDate.ToString(),
                        clearanceExpiredOn = "NA",
                        //licenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString(),
                        licenseNo = resp.FirstOrDefault().LicenceNumber,
                        clearanceFile = resp.FirstOrDefault().AppActionType != 200 ? "NA" : "NA",
                        statusDate = resp.FirstOrDefault().ActionDate,
                        integrationSource = "LABOUR",
                        deemedApproval = "false",
                        departmentTakenTotalTime = totalTakenTimeInfo.FirstOrDefault().TotalTime,
                        timeType = totalTakenTimeInfo.FirstOrDefault().TimeType,
                        appActionLogId = resp.FirstOrDefault().AppActionLogId,
                    };

                    BusinessFirstShareStatusLog businessFirstShareStatusLog = new BusinessFirstShareStatusLog()
                    {
                        ApplicationType = resp.FirstOrDefault().ApplicationType,
                        AppActionType = (AppActionTypeEnum)resp.FirstOrDefault().AppActionType,
                        ApiURL = _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value,
                        StatusSentOn = DateTime.Now,
                        RequestJSON = JsonConvert.SerializeObject(requestData),
                        IsRequestCompeleted = false,
                        RequestCompletionOn = null,
                        ResponseJson = "",
                        TriedCount = resp.FirstOrDefault().TriedCount + 1,
                        AppRefId = appRefId,
                        SyncStatusProcessEngineRefId = null
                    };

                    _context.BusinessFirstShareStatusLogs.Add(businessFirstShareStatusLog);
                    _context.SaveChanges();

                    // Post Request data to Invest punjab
                    string tokenurl = _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetBusinessFirstTokenUrl").Value;
                    string integrationKey = _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("InvestPunjabApiTokenIntegrationKey").Value;
                    InvestPunjabApiTokenViewModel investPunjabApiToken = null;
                    using (var httpClient = new HttpClient())
                    {
                        var tokenRequestData = new
                        {
                            IntegrationKey = integrationKey
                        };

                        StringContent content = new StringContent(JsonConvert.SerializeObject(tokenRequestData), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(tokenurl, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            investPunjabApiToken = JsonConvert.DeserializeObject<InvestPunjabApiTokenViewModel>(apiResponse);
                        }
                    }

                    if (investPunjabApiToken != null && investPunjabApiToken.Token != null)
                    {
                        string apiUrl = _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value;
                        using (var httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Add("authorization", investPunjabApiToken.Token);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync(apiUrl, content))
                            {
                                businessFirstShareStatusLog.RequestCompletionOn = DateTime.Now;
                                businessFirstShareStatusLog.IsRequestCompeleted = true;
                                businessFirstShareStatusLog.ResponseJson = await response.Content.ReadAsStringAsync();
                                _context.BusinessFirstShareStatusLogs.Update(businessFirstShareStatusLog);
                                _context.SaveChanges();
                            }
                        }
                    }

                    await MapRootActivity(new ToDoTableRootActivityMapping()
                    {
                        EntityModelName = "BusinessFirstShareStatusLog",
                        PrimaryKeyValue = appRefId,
                        RootActivityRefId = rootActivityRefId,
                        ToDoCodeType = toDoCodeType
                    });
                }
            }
            return true;
        }


        public async Task<bool> UpdateCircleRefIdInProjectSite(Int64 projectSiteRefId, int projectSiteVersion, Int64 circleRefId, CircleTypeEnum circleType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteRefId).FirstOrDefault();
            var projectSiteLogs = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == projectSiteRefId && x.ProjectSiteVersion == projectSiteVersion).FirstOrDefault();
            if (circleType == CircleTypeEnum.FACTORY_CIRCLE)
            {
                projectSite.FactoryCircleRefId = circleRefId;
                projectSiteLogs.FactoryCircleRefId = circleRefId;
            }
            else if (circleType == CircleTypeEnum.LABOUR_CIRCLE)
            {
                projectSite.LabourCircleRefId = circleRefId;
                projectSiteLogs.LabourCircleRefId = circleRefId;
            }
            else if (circleType == CircleTypeEnum.ALC_CIRCLE)
            {
                projectSite.AlcCircleRefId = circleRefId;
                projectSiteLogs.AlcCircleRefId = circleRefId;
            }
            _context.Update<ProjectSite>(projectSite);
            _context.SaveChanges();

            _context.Update<ProjectSiteLog>(projectSiteLogs);
            _context.SaveChanges();


            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ProjectSite",
                PrimaryKeyValue = projectSiteRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> UpdateBusinessFirstRequestNativeAppIdByIPin(Int64 iPin, Int64 investPunjabAppId, Int64 nativeAppId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var foundService = ApplicationLevelCommonOps.FindServiceCodeByApplicationType_PurposeType(applicationType, applicationPurposeType);
            var foundLogs = await _context.BusinessFirst_RequestLogs.Where(x => x.IPin == iPin && x.AppId == investPunjabAppId && x.ServiceCode == foundService.ServiceCode).ToListAsync();
            if (foundLogs.Count() > 0) //Existing record
            {
                var logs = foundLogs.Select(x => { x.LastModifiedDate = DateTime.Now; x.NativeAppId = nativeAppId; return x; }).ToList();
                await _context.BulkUpdateAsync<BusinessFirst_RequestLog>(logs);
            }

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "BusinessFirst_RequestLog",
                PrimaryKeyValue = nativeAppId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }


        public async Task<GenericResponseTemplateModel<List<ToDoUserWiseActivityViewModel>>> GetUserWiseActivities()
        {
            GenericResponseTemplateModel<List<ToDoUserWiseActivityViewModel>> genericRespModel = new GenericResponseTemplateModel<List<ToDoUserWiseActivityViewModel>>();
            try
            {
                //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                //{
                //    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=ProjectSiteRefId.ToString(), isNumber=false}
                //};
                genericRespModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<ToDoUserWiseActivityViewModel>("dbo.sp_ToDo_GetUserWiseActivityData");
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<GenericResponseTemplateModel<List<ToDoActivityLogViewModel>>> GetActivitiesByRootActivityId(DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<ToDoActivityLogViewModel>> genericRespModel = new GenericResponseTemplateModel<List<ToDoActivityLogViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RootActivityRefIds", ParmValue=dataTableParams.RootActivityRefIds, isNumber=false},
                    new StoreProcedureParm (){ isNumber=false, ParmName="SearchCode",ParmValue= dataTableParams.SearchCode==null ? "": dataTableParams.SearchCode},
                    new StoreProcedureParm (){ isNumber=true, ParmName="PageNo", ParmValue=dataTableParams.PageNo.ToString() },
                    new StoreProcedureParm (){ isNumber=true, ParmName="PageSize", ParmValue=dataTableParams.PageSize.ToString()},
                    new StoreProcedureParm (){ isNumber=false, ParmName="SortColumn", ParmValue=dataTableParams.SortColumn.ToString()},
                    new StoreProcedureParm (){ isNumber=false, ParmName="SortOrder", ParmValue = dataTableParams.SortOrder=="1" ? "ASC" : "DESC" },
                    new StoreProcedureParm (){ isNumber=false, ParmName="FilterArray", ParmValue = dataTableParams.FilterArray }

                };
                genericRespModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ToDoActivityLogViewModel>("dbo.sp_ToDo_GetRootActivityIdWiseData", storeProcedureParms);

                if (genericRespModel.ResponseDataModel.Count() > 0)
                {
                    genericRespModel.ResponseDataModel.Select(
                     x =>
                     {
                         x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         x.ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), x.ApplicationPurposeType));
                         //x.ToDoCodeTypeDesc = EnumOps.GetEnumDescriptionName<ToDoCodeTypeEnum>(Enum.GetName(typeof(ToDoCodeTypeEnum), x.ToDoCodeType));
                         x.ToDoActivityModeTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityModeTypeEnum>(Enum.GetName(typeof(ToDoActivityModeTypeEnum), x.ToDoActivityModeType));
                         //x.ToDoActivityCompleteTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCompleteTypeEnum>(Enum.GetName(typeof(ToDoActivityCompleteTypeEnum), x.ToDoActivityCompleteType));
                         x.ToDoActivityCategoryTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCategoryTypeEnum>(Enum.GetName(typeof(ToDoActivityCategoryTypeEnum), x.ToDoActivityCategoryType));
                         return x;
                     }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }
        public async Task<GenericResponseTemplateModel<List<ToDoActivityWiseStepViewModel>>> GetActivitiesWiseSteps(string rootActivityRefId)
        {
            GenericResponseTemplateModel<List<ToDoActivityWiseStepViewModel>> genericRespModel = new GenericResponseTemplateModel<List<ToDoActivityWiseStepViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RootActivityRefId", ParmValue=rootActivityRefId, isNumber=false}
                };
                genericRespModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ToDoActivityWiseStepViewModel>("dbo.sp_ToDo_GetRootActivityIdStepsData", storeProcedureParms);

                if (genericRespModel.ResponseDataModel.Count() > 0)
                {
                    genericRespModel.ResponseDataModel.Select(
                     x =>
                     {
                         x.ToDoCodeTypeDesc = EnumOps.GetEnumDescriptionName<ToDoCodeTypeEnum>(Enum.GetName(typeof(ToDoCodeTypeEnum), x.ToDoCodeType));
                         x.ToDoActivityCompleteTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCompleteTypeEnum>(Enum.GetName(typeof(ToDoActivityCompleteTypeEnum), x.ToDoActivityCompleteType));
                         return x;
                     }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }


        public async Task<GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>> GetOpenedTicketsByUserId(string userRefId)
        {
            GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>> genericRespModel = new GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userRefId, isNumber=false}
                };
                genericRespModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ToDoTicketDetailViewModel>("dbo.sp_ToDo_GetOpenedTicketsByUserId", storeProcedureParms);

                if (genericRespModel.ResponseDataModel.Count() > 0)
                {
                    genericRespModel.ResponseDataModel.Select(
                     x =>
                     {
                         x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         x.ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), x.ApplicationPurposeType));
                         return x;
                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>> GetAssignedTicketsByManpowerUserId(string manpowerUserId, ToDoTicketStatusTypeEnum toDoTicketStatusType)
        {
            GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>> genericRespModel = new GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ManpowerUserId", ParmValue=manpowerUserId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ToDoTicketStatusType", ParmValue=((int)toDoTicketStatusType).ToString(), isNumber=true}
                };
                genericRespModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ToDoTicketDetailViewModel>("dbo.sp_ToDo_GetAssignedTicketsByManpowerUserId", storeProcedureParms);

                if (genericRespModel.ResponseDataModel.Count() > 0)
                {
                    genericRespModel.ResponseDataModel.Select(
                     x =>
                     {
                         x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         x.ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), x.ApplicationPurposeType));
                         x.ToDoTicketStatusTypeDesc = EnumOps.GetEnumDescriptionName<ToDoTicketStatusTypeEnum>(Enum.GetName(typeof(ToDoTicketStatusTypeEnum), x.ToDoTicketStatusType));
                         x.ToDoActivityCategoryTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCategoryTypeEnum>(Enum.GetName(typeof(ToDoActivityCategoryTypeEnum), x.ToDoActivityCategoryType));
                         return x;
                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<bool> RaiseTicket(string userRefId, string rootActivityRefId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType, ToDoActivityCategoryTypeEnum toDoActivityCategoryType, Int64 investPunjab_Ipin, Int64 investPunjab_AppId)
        {
            try
            {
                var manpowerMapping = _context.ToDoApplicationWiseManpowerMappings.Where(x => x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType).FirstOrDefault();
                await _context.AddAsync<ToDoTicket>(new ToDoTicket()
                {
                    CreatedOn = DateTime.Now,
                    ManpowerMappingRefId = manpowerMapping != null ? manpowerMapping.Id : 0,
                    RootActivityRefId = rootActivityRefId,
                    ToDoTicketStatusType = ToDoTicketStatusTypeEnum.OPEN
                });

                await _context.SaveChangesAsync();

                //Notification : To Applicant
                var smsTemplateText = $"New Ticket (#" + rootActivityRefId + ") has been created.";
                var templateId = "1407172725558167954";
                TemplateTicketCreationToApplicantNotificationViewModel mailTemplate = new TemplateTicketCreationToApplicantNotificationViewModel()
                {
                    RootActivityRefId = rootActivityRefId
                };
                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateTicketCreationToApplicantNotification", mailTemplate);
                await _iNotificationManagerService.InitiateNotification(userRefId,
                new List<NotificationViewModel>()
                {
                new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New Ticket (#"+ rootActivityRefId + ") has been created."},
                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                });



                var application = _context.Applications.Where(x => x.InvestPunjab_Ipin == investPunjab_Ipin.ToString()
                   && x.IsDeleted == false
                   && x.InvestPunjab_AppId == investPunjab_AppId).Include(x => x.ProjectSites).FirstOrDefault();

                //Notification : To Developer
                var smsTemplateText1 = $"New Ticket (#" + rootActivityRefId + ") has been assigned.";
                var templateId1 = "1407172725558167954";


                var developerDetail = await _iAuthService.GetUserProfileByUserId(manpowerMapping.ManpowerUserRefId);
                var userDetail = await _iAuthService.GetUserProfileByUserId(userRefId);

                string estbName = "Not Available";
                var projectSiteMapping = _context.ProjectSite_IpinMappings.Where(x => x.IPin == investPunjab_Ipin.ToString()).FirstOrDefault();
                if (projectSiteMapping != null)
                {
                    estbName = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteMapping.ProjectSiteRefId).FirstOrDefault().EstablishmentName;
                }

                var steps = await GetActivitiesWiseSteps(rootActivityRefId);
                TemplateTicketCreationToDevTeamNotificationViewModel mailTemplate1 = new TemplateTicketCreationToDevTeamNotificationViewModel()
                {
                    RootActivityRefId = rootActivityRefId,

                    ApplicantName = userDetail.FirstName + " " + userDetail.LastName,
                    UserName = userDetail.UserProfileMapping.User.UserName,

                    ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), applicationPurposeType)),
                    ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), applicationType)),
                    AppRefId = application.AppId,

                    EstablishmentName = application.ProjectSites.EstablishmentName,
                    InvestPunjab_AppId = investPunjab_AppId.ToString(),
                    InvestPunjab_Ipin = investPunjab_Ipin.ToString(),
                    ToDoActivityCategoryTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCategoryTypeEnum>(Enum.GetName(typeof(ToDoActivityCategoryTypeEnum), toDoActivityCategoryType)),

                    DeveloperName = developerDetail.FirstName + " " + developerDetail.LastName,
                    CreatedOn = DateTime.Now,
                    ToDoActivityWiseSteps = steps.ResponseDataModel
                };
                var emailTemplateText1 = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateTicketCreationToDevTeamNotification", mailTemplate1);
                await _iNotificationManagerService.InitiateNotification(manpowerMapping.ManpowerUserRefId,
                new List<NotificationViewModel>()
                {
                new NotificationViewModel(){Body=emailTemplateText1, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New Ticket (#"+ rootActivityRefId + ") has been assigned."},
                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId1}
                });




                //Notification : To Helpdesk
                var smsTemplateText2 = $"New Ticket (#" + rootActivityRefId + ") has been assigned.";
                var templateId2 = "1407172725558167954";
                var helpDeskUserRefId = "F96930BF-3336-4421-87A9-D95D5B2F0AAF";
                if (projectSiteMapping != null)
                {
                    estbName = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteMapping.ProjectSiteRefId).FirstOrDefault().EstablishmentName;
                }

                TemplateTicketCreationToDevTeamNotificationViewModel mailTemplate2 = new TemplateTicketCreationToDevTeamNotificationViewModel()
                {
                    RootActivityRefId = rootActivityRefId,

                    ApplicantName = userDetail.FirstName + " " + userDetail.LastName,
                    UserName = userDetail.UserProfileMapping.User.UserName,

                    ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), applicationPurposeType)),
                    ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), applicationType)),
                    AppRefId = application.AppId,

                    EstablishmentName = application.ProjectSites.EstablishmentName,
                    InvestPunjab_AppId = investPunjab_AppId.ToString(),
                    InvestPunjab_Ipin = investPunjab_Ipin.ToString(),
                    ToDoActivityCategoryTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCategoryTypeEnum>(Enum.GetName(typeof(ToDoActivityCategoryTypeEnum), toDoActivityCategoryType)),

                    DeveloperName = developerDetail.FirstName + " " + developerDetail.LastName,
                    CreatedOn = DateTime.Now
                };
                var emailTemplateText2 = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateTicketCreationToHelpDeskTeamNotification", mailTemplate2);
                await _iNotificationManagerService.InitiateNotification(helpDeskUserRefId,
                new List<NotificationViewModel>()
                {
                new NotificationViewModel(){Body=emailTemplateText2, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New Ticket (#"+ rootActivityRefId + ") has been assigned."},
                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId2}
                });
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public async Task<Int64> Master_Update(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            _context.Update(requestData);
            _context.SaveChanges();
            PropertyInfo entityKeyProp = requestData.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(KeyAttribute)));
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = requestData.GetType().Name,
                PrimaryKeyValue = Convert.ToInt64(entityKeyProp.GetValue(requestData)),
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });


            return Convert.ToInt64(entityKeyProp.GetValue(requestData));
        }

        public async Task<bool> UpdateLastModifiedDateInApplication(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var app = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            app.LastModifiedOnDate = DateTime.Now;
            _context.Update(app);
            await _context.SaveChangesAsync();
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SetIsDirtyFlag(Int64 appRefId, bool flagValue, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
            {
                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true },
                new StoreProcedureParm (){ ParmName="IsDirty", ParmValue= flagValue ? "1" : "0", isNumber=false }
            };
            var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SetApplicationIsDirtyStatus", storeProcedureParms2);

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationDirtyStatusMapping",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }
        public async Task<bool> MapRootActivity(ToDoTableRootActivityMapping obj)
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetApplicationLock(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var app = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            app.IsLocked = true;
            app.IsAllowEdit = false;
            _context.Update(app);
            await _context.SaveChangesAsync();
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SetApplicationLifeCycle(Int64 appRefId, ApplicationLifeCycleStatusTypeEnum applicationLifeCycleStatusType, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var app = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            app.ApplicationLifeCycleStatusType = applicationLifeCycleStatusType;
            app.ApplicationLifeCycleLastStatusOn = DateTime.Now;
            _context.Update(app);
            await _context.SaveChangesAsync();
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SendApplicationToLandingOfficer(Int64 appRefId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            Application application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE && application.IsFeeApplicable == false)
            {
                var factoryLicence = await _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefaultAsync();
                if (Convert.ToBoolean(factoryLicence.IsTempRegistered))
                {
                    appActionType = AppActionTypeEnum.FACTORY_APP_SUBMITTED_FEE_NOT_APPLICABLE;
                }
            }

            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
            {
                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                new StoreProcedureParm (){ ParmName="AppActionType", ParmValue= ((int)appActionType).ToString(), isNumber=true},
            };

            await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_SendApplicationToLendingOfficers", storeProcedureParms);

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationActionLog",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            if (application.IsTimeLineFlow && application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED
                || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING
                || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT
                || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                await _iAppTimeLineManagerService.SeedTimeLineData(appRefId, 0);
            }

            return true;
        }
        public async Task<bool> SendApplicationToLandingOfficerAfterObjectionResolved(Int64 appRefId, AppActionTypeEnum appActionType, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            Application application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();

            #region Calculate Days & Hours

            var applicationAction = new ApplicationAction();
            if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                var parentWithChildObject = await _context.ApplicationAction_ParallelProcesses.Where(x => x.ApplicationRefId == appRefId && x.IsAlive).OrderByDescending(x => x.AppActionParallelProcessId).FirstOrDefaultAsync();
                applicationAction = new ApplicationAction
                {
                    ApplicationRefId = parentWithChildObject.ApplicationRefId,
                    ActionDate = parentWithChildObject.ActionDate,
                };
            }
            else
            {
                applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).FirstOrDefaultAsync();
            }

            int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
            int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
            int totalWorkingDays = 0;
            if (holidays > 0)
            {
                totalWorkingDays = (totalDays - holidays);
            }
            else
            {
                totalWorkingDays = totalDays;
            }

            var end = DateTime.Now;
            var start = applicationAction.ActionDate;
            var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

            var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                 .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));
            #endregion

            List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
            {
                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                new StoreProcedureParm (){ ParmName="AppActionType", ParmValue= ((int)appActionType).ToString(), isNumber=true},

                new StoreProcedureParm (){ ParmName="TotalWorkingDays", ParmValue= totalWorkingDays.ToString(), isNumber=true},
                new StoreProcedureParm (){ ParmName="TotalWorkingHours", ParmValue= totalWorkingHours.ToString(), isNumber=true},
                new StoreProcedureParm (){ ParmName="Remarks", ParmValue= remarks.ToString(), isNumber=false}
            };

            if (application.IsTimeLineFlow)
            {
                await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_TimeLine_SendAppToOfficerAfterObjResolve", storeProcedureParm);
            }
            else
            {
                await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_SendAppToOfficerAfterObjResolve", storeProcedureParm);
            }

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationActionLog",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            if (application.IsTimeLineFlow)
            {
                await _iAppTimeLineManagerService.SeedTimeLineData(appRefId, 0);
            }


            return true;
        }

        public async Task<bool> SET_ISHAVING_EMPLOYEE_FLAG(Int64 shopLicenceId, int isHavingEmployee, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {

            var data = _context.ShopLicence_GeneralDetails.Where(x => x.ShopLicenceId == shopLicenceId).FirstOrDefault();
            data.IsHavingEmployee = isHavingEmployee;
            _context.Update<ShopLicence_GeneralDetail>(data);
            _context.SaveChanges();

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationActionLog",
                PrimaryKeyValue = shopLicenceId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<GenericResponseTemplateModel<List<ToDoApplicationActivityMaping>>> GetActivityMappingList()
        {
            GenericResponseTemplateModel<List<ToDoApplicationActivityMaping>> genericRespModel = new GenericResponseTemplateModel<List<ToDoApplicationActivityMaping>>();
            try
            {
                genericRespModel.ResponseDataModel = await _context.ToDoApplicationActivityMapings
                    .OrderBy(x => x.ApplicationType)
                    .ThenBy(x => x.ApplicationPurposeType)
                    .ThenBy(x => x.ToDoActivityCategoryType)
                    .ThenBy(x => x.ToDoActivityModeType)
                    .ThenBy(x => x.ToDoSerialOrderCount)
                    .ToListAsync();

                if (genericRespModel.ResponseDataModel.Count() > 0)
                {
                    genericRespModel.ResponseDataModel.Select(
                     x =>
                     {
                         x.ToDoCodeTypeDesc = EnumOps.GetEnumDescriptionName<ToDoCodeTypeEnum>(Enum.GetName(typeof(ToDoCodeTypeEnum), x.ToDoCodeType));
                         x.ApplicationTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(Enum.GetName(typeof(ApplicationTypeEnum), x.ApplicationType));
                         x.ApplicationPurposeTypeDesc = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(Enum.GetName(typeof(ApplicationPurposeTypeEnum), x.ApplicationPurposeType));
                         x.ToDoActivityModeTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityModeTypeEnum>(Enum.GetName(typeof(ToDoActivityModeTypeEnum), x.ToDoActivityModeType));
                         x.ToDoActivityCategoryTypeDesc = EnumOps.GetEnumDescriptionName<ToDoActivityCategoryTypeEnum>(Enum.GetName(typeof(ToDoActivityCategoryTypeEnum), x.ToDoActivityCategoryType));
                         return x;
                     }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<bool> SET_REG_REN_AMD_DATES(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            if (requestData is Licence_Factory_GeneralDetail formModel)
            {
                if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    formModel.RenewalFromDate = null;
                    formModel.OldLicenceValidUpTo = null;
                    formModel.RegistrationDate = new DateTime(formModel.RegistrationDate_Json.year, formModel.RegistrationDate_Json.month, formModel.RegistrationDate_Json.day);
                }
                else if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    formModel.RenewalFromDate = new DateTime(formModel.RenewalFromDate_Json.year, formModel.RenewalFromDate_Json.month, formModel.RenewalFromDate_Json.day);
                    formModel.OldLicenceValidUpTo = new DateTime(formModel.OldLicenceValidUpTo_Json.year, formModel.OldLicenceValidUpTo_Json.month, formModel.OldLicenceValidUpTo_Json.day);
                    formModel.RegistrationDate = null;
                }
                else
                {
                    formModel.RenewalFromDate = null;
                    formModel.OldLicenceValidUpTo = new DateTime(formModel.OldLicenceValidUpTo_Json.year, formModel.OldLicenceValidUpTo_Json.month, formModel.OldLicenceValidUpTo_Json.day);
                    formModel.RegistrationDate = null;
                }
            }
            return true;
        }

        public async Task<bool> SET_AMENDMENT_HISTORY_COUNTER(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            Licence_Factory_GeneralDetail previousFactoryLicenseData = null;
            List<Licence_Factory_AmendmentDataHistory> savedAmendmentDataHistories = null;

            previousFactoryLicenseData = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).AsNoTracking().FirstOrDefault();
            savedAmendmentDataHistories = await _context.Licence_Factory_AmendmentDataHistories.Where(x => !x.IsLocked
                                          && x.ModifiedCounter == previousFactoryLicenseData.ModifiedCounter
                                          && (x.FieldName == "PowerKW_Installed" || x.FieldName == "Workers_MaxDuringYear")).ToListAsync();

            if (savedAmendmentDataHistories.Count() > 0)
            {
                previousFactoryLicenseData.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter + 1;
            }
            else
            {
                previousFactoryLicenseData.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter;
            }

            _context.Update(previousFactoryLicenseData);
            await _context.SaveChangesAsync();

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationDirtyStatusMapping",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public async Task<bool> PREPARE_DATA_AS_PER_TEMP_REG_FLAG(Int64 appRefId, Int64 entityKeyId, string tempRegistrationNumber, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {

            GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel>();
            var licence = await _iDapperRepository.Get<Factory_TemporaryLicenceDetailsViewModel>("[dbo].[sp_GetTemporaryLicenceDetails_MasterData]", new { TempRegistrationNumber = tempRegistrationNumber }, CommandType.StoredProcedure).ConfigureAwait(false);
            genericServiceResultTemplate.ResponseDataModel = licence.FirstOrDefault();

            Licence_Factory_OccupierAndManagerDetail licence_Factory_OccupierAndManagerDetail = new Licence_Factory_OccupierAndManagerDetail()
            {
                ManagerFullName = genericServiceResultTemplate.ResponseDataModel.ManagerFullName,
                ManagerFatherName = genericServiceResultTemplate.ResponseDataModel.ManagerFatherName,
                ManagerFullAddress = genericServiceResultTemplate.ResponseDataModel.ManagerFullAddress,
                ManagerMobile = genericServiceResultTemplate.ResponseDataModel.ManagerMobile,
                ManagerEmail = genericServiceResultTemplate.ResponseDataModel.ManagerEmail,
                ManagerResidentialAddress = genericServiceResultTemplate.ResponseDataModel.ManagerFullAddress,
                OccupierFullName = genericServiceResultTemplate.ResponseDataModel.OccupierFullName,
                OccupierFatherName = genericServiceResultTemplate.ResponseDataModel.OccupierFatherName,
                OccupierFullAddress = genericServiceResultTemplate.ResponseDataModel.OccupierFullAddress,
                OccupierMobile = genericServiceResultTemplate.ResponseDataModel.OccupierMobile,
                OccupierEmail = genericServiceResultTemplate.ResponseDataModel.OccupierEmail,
                OccupierResidentialAddress = genericServiceResultTemplate.ResponseDataModel.OccupierFullAddress,
                OwnerName = genericServiceResultTemplate.ResponseDataModel.OwnerName,
                OwnerPremisesAddress = genericServiceResultTemplate.ResponseDataModel.OwnerPremisesAddress,
                StabilityCertificateNumber = "NA",
                StabilityCertificateDate = DateTime.Now,
                StabilityDOFNumber = genericServiceResultTemplate.ResponseDataModel.ReferenceNumberBuildingConst,
                Checklist_IsBuildingPlanApproved = "0",
                Checklist_IsLabourWelfareFundPaid = "0",
                Checklist_IsAnnualReturnFiled = "0",
                Checklist_IsStabilityCertificateAttached = "0",
                ModifiedCounter = 1,
                FactoryLicenceRefId = entityKeyId
            };
            await _context.Licence_Factory_OccupierAndManagerDetails.AddAsync(licence_Factory_OccupierAndManagerDetail);
            await _context.SaveChangesAsync();

            AppFeeDetail appFeeDetail = new AppFeeDetail()
            {
                AppRefId = appRefId,
                FeeHeaderRefId = 31,
                Amount = genericServiceResultTemplate.ResponseDataModel.TXN_AMOUNT,
                CalculatedOn = genericServiceResultTemplate.ResponseDataModel.PaymentDate,
                IsDeduductible = false,
                PaymentBatchCounter = 1,
                PaymentPartCounter = 0,
                HasDedicatedTreasuryCode = false,
                DedicatedTreasurCode = null,
                DedicatedDDOCode = null,
                Description = null,
            };
            await _context.AppFeeDetails.AddAsync(appFeeDetail);
            await _context.SaveChangesAsync();

            AppFeeTransaction appFeeTransaction = new AppFeeTransaction()
            {
                TransactionInitializationDate = genericServiceResultTemplate.ResponseDataModel.PaymentDate,
                PaymentGatewayType = PaymentGatewayTypeEnum.IFMS,
                PaymentModeType = PaymentModeTypeEnum.ONLINE,
                PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY,
                PaymentGatewayTargetUrl = "NA",
                PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST,
                UniquePaymentGatewayTransactionId = genericServiceResultTemplate.ResponseDataModel.ME_TXN_REF_NO,
                IsWebRequestCycleCompleted = true,
                RequestBodyData = genericServiceResultTemplate.ResponseDataModel.ReqMsg,
                ResponseBodyData = genericServiceResultTemplate.ResponseDataModel.ResMsg,
                ResponseReceivedOn = genericServiceResultTemplate.ResponseDataModel.PaymentDate,
                ResponseMessage = genericServiceResultTemplate.ResponseDataModel.ResMsg,
                TransactionFinalStatusType = TransactionFinalStatusTypeEnum.SUCCEED,
                AmountCalculated = genericServiceResultTemplate.ResponseDataModel.TXN_AMOUNT,
                PaymentBatchCounter = 1,
                PaymentPartCounter = 1,
                BankTransactionRefNumber1 = genericServiceResultTemplate.ResponseDataModel.BANK_REFERENCE_NO,
                BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                BankTransactionRefNumber2 = genericServiceResultTemplate.ResponseDataModel.BANK_REFERENCE_NO,
                BankTransactionRefNumber2_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                BankSettlementOn = genericServiceResultTemplate.ResponseDataModel.PaymentDate,
                AppRefId = appRefId
            };
            await _context.AppFeeTransactions.AddAsync(appFeeTransaction);
            await _context.SaveChangesAsync();

            AppPaymentSuccessTransactionMapping appPaymentSuccessTransactionMapping = new AppPaymentSuccessTransactionMapping()
            {
                AppRefId = appRefId,
                AppFeeTransactionRefId = appFeeTransaction.AppFeeTransactionId,
                PaymentBatchCounter = 1,
                PaymentPartCounter = 1,
            };
            await _context.AppPaymentSuccessTransactionMappings.AddAsync(appPaymentSuccessTransactionMapping);
            await _context.SaveChangesAsync();

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationDirtyStatusMapping",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public async Task<bool> PREPARE_AND_SET_AMENDMENT_HISTORY_DATA(object requestData, Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            Licence_Factory_GeneralDetail previousFactoryLicenseData = null;
            List<Licence_Factory_AmendmentDataHistory> savedAmendmentDataHistories = null;

            Licence_CL_PE_GeneralDetail previousPELicenseData = null;
            List<Licence_PE_AmendmentDataHistories> savedPEAmendmentDataHistories = null;

            Licence_ContractLabour_GeneralDetail previousContractLicenseData = null;
            List<Licence_ContractLabour_AmendmentDataHistory> savedCLAmendmentDataHistories = null;

            Licence_PE_ISM_GeneralDetail previousISMPELicenseData = null;
            List<Licence_PE_ISM_AmendmentDataHistories> savedISMPEAmendmentDataHistories = null;

            if (requestData is Licence_Factory_GeneralDetail formModel)
            {
                previousFactoryLicenseData = _context.Licence_Factory_GeneralDetails.Where(x => x.FactoryLicenceId == formModel.FactoryLicenceId).AsNoTracking().FirstOrDefault();
                savedAmendmentDataHistories = await _context.Licence_Factory_AmendmentDataHistories.Where(x =>
                           !x.IsLocked &&
                           x.ModifiedCounter == previousFactoryLicenseData.ModifiedCounter &&
                           (x.FieldName == "PowerKW_Installed" || x.FieldName == "Workers_MaxDuringYear")).ToListAsync();

                if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousFactoryLicenseData != null)
                {
                    List<Licence_Factory_AmendmentDataHistory> amendmentDataHistories = new List<Licence_Factory_AmendmentDataHistory>();


                    if (savedAmendmentDataHistories.Any(x => x.FieldName == "Workers_MaxDuringYear"))
                    {
                        if (Convert.ToInt64(savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").Select(x => x.PreviousValue).FirstOrDefault()) == formModel.Workers_MaxDuringYear)
                        {
                            _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").FirstOrDefault());
                        }
                        else
                        {
                            var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").FirstOrDefault();
                            amdHistory.ModifiedValue = formModel.Workers_MaxDuringYear.ToString();
                            _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousFactoryLicenseData.Workers_MaxDuringYear != formModel.Workers_MaxDuringYear)
                        {
                            amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                            {
                                AppRefId = formModel.AppRefId,
                                FieldName = "Workers_MaxDuringYear",
                                PreviousValue = previousFactoryLicenseData.Workers_MaxDuringYear.ToString(),
                                ModifiedValue = formModel.Workers_MaxDuringYear.ToString(),
                                SectionCode = "MP",
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel.ModifiedCounter
                            });
                        }
                    }

                    if (savedAmendmentDataHistories.Any(x => x.FieldName == "PowerKW_Installed"))
                    {
                        if (Convert.ToInt64(savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").Select(x => x.PreviousValue).FirstOrDefault()) == formModel.PowerKW_Installed)
                        {
                            _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").FirstOrDefault());
                        }
                        else
                        {
                            var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").FirstOrDefault();
                            amdHistory.ModifiedValue = formModel.PowerKW_Installed.ToString();
                            _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousFactoryLicenseData.PowerKW_Installed != formModel.PowerKW_Installed)
                        {
                            amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                            {
                                AppRefId = formModel.AppRefId,
                                FieldName = "PowerKW_Installed",
                                PreviousValue = previousFactoryLicenseData.PowerKW_Installed.ToString(),
                                ModifiedValue = formModel.PowerKW_Installed.ToString(),
                                SectionCode = "KW",
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel.ModifiedCounter
                            });
                        }
                    }
                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_Factory_AmendmentDataHistory>(amendmentDataHistories);
                    }
                }
            }
            else if (requestData is Licence_CL_PE_GeneralDetail formModel1)
            {
                var application = _context.Applications.Where(x => x.AppId == formModel1.AppRefId && x.IsDeleted == false).FirstOrDefault();


                previousPELicenseData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.Id == formModel1.Id).AsNoTracking().FirstOrDefault();


                savedPEAmendmentDataHistories = await _context.Licence_PE_AmendmentDataHistories.Where(x =>
                        !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId && x.FieldName != "TotalWorker").ToListAsync();



                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    List<Licence_PE_AmendmentDataHistories> amendmentDataHistories = new List<Licence_PE_AmendmentDataHistories>();

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerName"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.PE_Name)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.PE_Name.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.PE_Name != formModel1.PE_Name)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "PrincipalEmployerName",
                                PreviousValue = previousPELicenseData.PE_Name.ToString(),
                                ModifiedValue = formModel1.PE_Name.ToString(),
                                SectionCode = "PED", //ConstructionBuildingDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_FatherName"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.PE_FatherName)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.PE_FatherName.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.PE_FatherName != formModel1.PE_FatherName)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "PE_FatherName",
                                PreviousValue = previousPELicenseData.PE_FatherName.ToString(),
                                ModifiedValue = formModel1.PE_FatherName.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Mobile"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.PE_Mobile)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.PE_Mobile.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.PE_Mobile != formModel1.PE_Mobile)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "PE_Mobile",
                                PreviousValue = previousPELicenseData.PE_Mobile.ToString(),
                                ModifiedValue = formModel1.PE_Mobile.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Email"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.PE_Email)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.PE_Email.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.PE_Email != formModel1.PE_Email)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "PE_Email",
                                PreviousValue = previousPELicenseData.PE_Email.ToString(),
                                ModifiedValue = formModel1.PE_Email.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.PE_Address)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.PE_Address.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.PE_Address != formModel1.PE_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "PE_Address",
                                PreviousValue = previousPELicenseData.PE_Address.ToString(),
                                ModifiedValue = formModel1.PE_Address.ToString(),
                                SectionCode = "PED", //ContractLabourDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Name"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.Manager_Name)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.Manager_Name.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.Manager_Name != formModel1.Manager_Name)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "Manager_Name",
                                PreviousValue = previousPELicenseData.Manager_Name.ToString(),
                                ModifiedValue = formModel1.Manager_Name.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }


                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Mobile"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.Manager_Mobile)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.Manager_Mobile.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.Manager_Mobile != formModel1.Manager_Mobile)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "Manager_Mobile",
                                PreviousValue = previousPELicenseData.Manager_Mobile.ToString(),
                                ModifiedValue = formModel1.Manager_Mobile.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Email"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.Manager_Email)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.Manager_Email.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.Manager_Email != formModel1.Manager_Email)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "Manager_Email",
                                PreviousValue = previousPELicenseData.Manager_Email.ToString(),
                                ModifiedValue = formModel1.Manager_Email.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.Manager_Address)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.Manager_Address.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.Manager_Address != formModel1.Manager_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "Manager_Address",
                                PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                ModifiedValue = formModel1.Manager_Address.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }
                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "NatureOfWork"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.NatureOfWork)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.NatureOfWork.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.NatureOfWork != formModel1.NatureOfWork)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "NatureOfWork",
                                PreviousValue = previousPELicenseData.NatureOfWork.ToString(),
                                ModifiedValue = formModel1.NatureOfWork.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }
                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel1.Manager_Address)
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel1.Manager_Address.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.Manager_Address != formModel1.Manager_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel1.AppRefId,
                                FieldName = "Manager_Address",
                                PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                ModifiedValue = formModel1.Manager_Address.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel1.ModifiedCounter
                            });
                        }
                    }


                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_PE_AmendmentDataHistories>(amendmentDataHistories);
                    }
                }
            }
            else if (requestData is IContractorDetailViewModel formModel2)
            {
                var application = _context.Applications.Where(x => x.AppId == formModel2.AppRefId && x.IsDeleted == false).FirstOrDefault();


                previousPELicenseData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.AppRefId == formModel2.AppRefId).AsNoTracking().FirstOrDefault();

                savedPEAmendmentDataHistories = await _context.Licence_PE_AmendmentDataHistories.Where(x =>
                        !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId && x.FieldName == "TotalWorker").ToListAsync();

                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    List<Licence_PE_AmendmentDataHistories> amendmentDataHistories = new List<Licence_PE_AmendmentDataHistories>();

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").Select(x => x.PreviousValue).FirstOrDefault() == formModel2.Totalworkers.ToString())
                        {
                            _context.Licence_PE_AmendmentDataHistories.Remove(savedPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel2.Totalworkers.ToString();
                            _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousPELicenseData.TotalWorker != formModel2.Totalworkers)
                        {
                            amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                            {
                                AppRefId = formModel2.AppRefId,
                                FieldName = "TotalWorker",
                                PreviousValue = previousPELicenseData.TotalWorker.ToString(),
                                ModifiedValue = formModel2.Totalworkers.ToString(),
                                SectionCode = "TW",
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = 1
                            });
                        }
                    }

                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_PE_AmendmentDataHistories>(amendmentDataHistories);
                    }

                    Licence_CL_PE_GeneralDetail previousPEData = null;
                    previousPEData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.AppRefId == formModel2.AppRefId).AsNoTracking().FirstOrDefault();
                    previousPEData.TotalWorker = formModel2.Totalworkers;
                    _context.Licence_CL_PE_GeneralDetail.Update(previousPEData);
                    await _context.SaveChangesAsync();

                }
            }
            else if (requestData is Licence_ContractLabour_GeneralDetail formModel3)
            {
                var application = _context.Applications.Where(x => x.AppId == formModel3.AppRefId && x.IsDeleted == false).FirstOrDefault();
                var licenceNo = _context.ApplicationLicenceNoMapping.Where(x => x.LicenceNumber == application.Legacy_LicenceNo).OrderByDescending(x => x.ApplicationLicenceNoMappingId).FirstOrDefault();
                previousContractLicenseData = _context.Licence_ContractLabour_GeneralDetails.Where(x => x.AppRefId == licenceNo.AppRefId).FirstOrDefault();

                savedCLAmendmentDataHistories = await _context.Licence_ContractLabour_AmendmentDataHistories.Where(x => x.AppRefId == previousContractLicenseData.AppRefId).ToListAsync();

                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE || application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    List<Licence_ContractLabour_AmendmentDataHistory> amendmentDataHistories = new List<Licence_ContractLabour_AmendmentDataHistory>();

                    if (savedCLAmendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                    {
                        if (savedCLAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").Select(x => x.PreviousValue).FirstOrDefault() == formModel3.MaximumNumberOfEmployee.ToString())
                        {
                            _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedCLAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedCLAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel3.MaximumNumberOfEmployee.ToString();
                            _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousContractLicenseData.MaximumNumberOfEmployee != formModel3.MaximumNumberOfEmployee)
                        {
                            amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                            {
                                AppRefId = formModel3.AppRefId,
                                FieldName = "TotalWorker",
                                PreviousValue = previousContractLicenseData.MaximumNumberOfEmployee.ToString(),
                                ModifiedValue = formModel3.MaximumNumberOfEmployee.ToString(),
                                SectionCode = "TW", //ConstructionBuildingDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel3.ModifiedCounter
                            });
                        }
                    }



                    if (savedCLAmendmentDataHistories.Any(x => x.FieldName == "Manager_Name"))
                    {
                        if (savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel3.AgentOrManagerName)
                        {
                            _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel3.AgentOrManagerName.ToString();
                            _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousContractLicenseData.AgentOrManagerName != formModel3.AgentOrManagerName)
                        {
                            amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                            {
                                AppRefId = formModel3.AppRefId,
                                FieldName = "Manager_Name",
                                PreviousValue = previousContractLicenseData.AgentOrManagerName.ToString(),
                                ModifiedValue = formModel3.AgentOrManagerName.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel3.ModifiedCounter
                            });
                        }
                    }


                    if (savedCLAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                    {
                        if (savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel3.AgentOrManagerAddress)
                        {
                            _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedCLAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel3.AgentOrManagerAddress.ToString();
                            _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousContractLicenseData.AgentOrManagerAddress != formModel3.AgentOrManagerAddress)
                        {
                            amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                            {
                                AppRefId = formModel3.AppRefId,
                                FieldName = "Manager_Address",
                                PreviousValue = previousContractLicenseData.AgentOrManagerAddress.ToString(),
                                ModifiedValue = formModel3.AgentOrManagerAddress.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel3.ModifiedCounter
                            });
                        }
                    }



                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_ContractLabour_AmendmentDataHistory>(amendmentDataHistories);
                    }
                }
            }
            else if (requestData is Licence_PE_ISM_GeneralDetail formModel4)
            {
                var application = _context.Applications.Where(x => x.AppId == formModel4.AppRefId && x.IsDeleted == false).FirstOrDefault();


                previousISMPELicenseData = _context.Licence_PE_ISM_GeneralDetails.Where(x => x.Id == formModel4.Id).AsNoTracking().FirstOrDefault();

                savedISMPEAmendmentDataHistories = await _context.Licence_PE_ISM_AmendmentDataHistories.Where(x =>
                         !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId).ToListAsync();

                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    List<Licence_PE_ISM_AmendmentDataHistories> amendmentDataHistories = new List<Licence_PE_ISM_AmendmentDataHistories>();

                    if (savedISMPEAmendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                    {
                        if (savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.TotalWorker.ToString())
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_Name.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.TotalWorker != formModel4.TotalWorker)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "TotalWorker",
                                PreviousValue = previousPELicenseData.TotalWorker.ToString(),
                                ModifiedValue = formModel4.TotalWorker.ToString(),
                                SectionCode = "TW", //ConstructionBuildingDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }



                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerName"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.PE_Name)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_Name.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.PE_Name != formModel4.PE_Name)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "PrincipalEmployerName",
                                PreviousValue = previousPELicenseData.PE_Name.ToString(),
                                ModifiedValue = formModel4.PE_Name.ToString(),
                                SectionCode = "PED", //ConstructionBuildingDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_FatherName"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.PE_FatherName)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_FatherName.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.PE_FatherName != formModel4.PE_FatherName)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "PE_FatherName",
                                PreviousValue = previousPELicenseData.PE_FatherName.ToString(),
                                ModifiedValue = formModel4.PE_FatherName.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Mobile"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.PE_Mobile)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_Mobile.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.PE_Mobile != formModel4.PE_Mobile)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "PE_Mobile",
                                PreviousValue = previousPELicenseData.PE_Mobile.ToString(),
                                ModifiedValue = formModel4.PE_Mobile.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Email"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.PE_Email)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_Email.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.PE_Email != formModel4.PE_Email)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "PE_Email",
                                PreviousValue = previousPELicenseData.PE_Email.ToString(),
                                ModifiedValue = formModel4.PE_Email.ToString(),
                                SectionCode = "PED", //ConstructionSiteDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "PE_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.PE_Address)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.PE_Address.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.PE_Address != formModel4.PE_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "PE_Address",
                                PreviousValue = previousPELicenseData.PE_Address.ToString(),
                                ModifiedValue = formModel4.PE_Address.ToString(),
                                SectionCode = "PED", //ContractLabourDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Name"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.Manager_Name)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.Manager_Name.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.Manager_Name != formModel4.Manager_Name)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "Manager_Name",
                                PreviousValue = previousPELicenseData.Manager_Name.ToString(),
                                ModifiedValue = formModel4.Manager_Name.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }


                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Mobile"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.Manager_Mobile)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.Manager_Mobile.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.Manager_Mobile != formModel4.Manager_Mobile)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "Manager_Mobile",
                                PreviousValue = previousPELicenseData.Manager_Mobile.ToString(),
                                ModifiedValue = formModel4.Manager_Mobile.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Email"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.Manager_Email)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.Manager_Email.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.Manager_Email != formModel4.Manager_Email)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "Manager_Email",
                                PreviousValue = previousPELicenseData.Manager_Email.ToString(),
                                ModifiedValue = formModel4.Manager_Email.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }

                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.Manager_Address)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.Manager_Address.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.Manager_Address != formModel4.Manager_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "Manager_Address",
                                PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                ModifiedValue = formModel4.Manager_Address.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }
                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "NatureOfWork"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.NatureOfWork)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.NatureOfWork.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.NatureOfWork != formModel4.NatureOfWork)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "NatureOfWork",
                                PreviousValue = previousPELicenseData.NatureOfWork.ToString(),
                                ModifiedValue = formModel4.NatureOfWork.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }
                    if (savedPEAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                    {
                        if (savedPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel4.Manager_Address)
                        {
                            _context.Licence_PE_ISM_AmendmentDataHistories.Remove(savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                        }
                        else
                        {
                            var amendmentHistory = savedISMPEAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                            amendmentHistory.ModifiedValue = formModel4.Manager_Address.ToString();
                            _context.Update<Licence_PE_ISM_AmendmentDataHistories>(amendmentHistory);
                        }
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (previousISMPELicenseData.Manager_Address != formModel4.Manager_Address)
                        {
                            amendmentDataHistories.Add(new Licence_PE_ISM_AmendmentDataHistories()
                            {
                                AppRefId = formModel4.AppRefId,
                                FieldName = "Manager_Address",
                                PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                ModifiedValue = formModel4.Manager_Address.ToString(),
                                SectionCode = "MD", //ManagerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = formModel4.ModifiedCounter
                            });
                        }
                    }


                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_PE_ISM_AmendmentDataHistories>(amendmentDataHistories);
                    }
                }
            }
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationDirtyStatusMapping",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public async Task<bool> SET_IS_FEE_APPLICABLE_FLAG(Int64 appRefId, bool isFeeApplicable, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var app = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            app.IsFeeApplicable = isFeeApplicable;
            _context.Update(app);
            await _context.SaveChangesAsync();
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SEND_NOTIFICATION(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var application = await _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefaultAsync().ConfigureAwait(false);
            var appAction = new ApplicationAction();
            if (application.ApplicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS)
            {
                return true;
            }
            if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                var parentWithChildObject = await _context.ApplicationAction_ParallelProcesses.Where(x => x.ApplicationRefId == appRefId && x.IsAlive == true).OrderByDescending(x => x.AppActionParallelProcessId).FirstOrDefaultAsync();
                appAction = JsonConvert.DeserializeObject<ApplicationAction>(JsonConvert.SerializeObject(parentWithChildObject));
            }
            else
            {
                appAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == appRefId).AsNoTracking().FirstOrDefaultAsync();
            }

            if ((AppActionTypeEnum)appAction.AppActionType == AppActionTypeEnum.APP_SUBMITTED_FEE_NOT_APPLICABLE || (AppActionTypeEnum)appAction.AppActionType == AppActionTypeEnum.FACTORY_APP_SUBMITTED_FEE_NOT_APPLICABLE)
            {
                var receiverInfo = await _iAuthService.GetUserProfileByUserId(appAction.Receiver_UserRefId);
                TemplateNewCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateNewCaseLendedToOfficerNotificationViewModel()
                {
                    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
                    EstablishmentName = application.ProjectSites.EstablishmentName,
                    PublicApplicationRefNo = application.PublicAppRefNum
                };
                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateNewCaseLendedToOfficerNotification", mailTemplate);
                await _iNotificationManagerService.InitiateNotification(appAction.Receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")"}
                });

                // Sent SMS Notification To User
                var smsTemplateText = $"Your application ({application.PublicAppRefNum}) has been submitted successfully -PB LABOUR";
                var templateId = "1407172724884749762";
                await _iNotificationManagerService.InitiateNotification(application.ProjectSites.UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                });


                // Sent SMS Notification To Officer
                var receiverUerName = await _context.Users.Where(x => x.Id == appAction.Receiver_UserRefId).Select(x => x.UserName).FirstOrDefaultAsync();
                var smsTemplateTexts = $"Dear User, Application no. ({application.PublicAppRefNum}) received in your {receiverUerName} account. Kindly Process it - PB LABOUR";
                var templateIds = "1407176009417429959";
                await _iNotificationManagerService.InitiateNotification(appAction.Receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=smsTemplateTexts, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateIds}
                });

            }
            else if ((AppActionTypeEnum)appAction.AppActionType == AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED)
            {
                var receiverInfo = await _iAuthService.GetUserProfileByUserId(appAction.Receiver_UserRefId);
                TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                {
                    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
                    EstablishmentName = application.ProjectSites.EstablishmentName,
                    PublicApplicationRefNo = application.PublicAppRefNum,
                    StatusDescription = "Objection Resolved And Application Re-submitted",
                    InvestPunjab_Ipin = "Ipin : " + application.InvestPunjab_Ipin + " / Application No. :" + application.InvestPunjab_AppId
                };
                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);

                await _iNotificationManagerService.InitiateNotification(appAction.Receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title="Department of Labour: Objection resolved and application re-submitted (" + mailTemplate.PublicApplicationRefNo + ")"}
                });

                // Sent SMS Notification To User
                var smsTemplateText = $"Your application ({application.PublicAppRefNum}) has been submitted successfully -PB LABOUR";
                var templateId = "1407172724884749762";
                await _iNotificationManagerService.InitiateNotification(application.ProjectSites.UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                });

                // Sent SMS Notification To Officer
                var receiverUerName = await _context.Users.Where(x => x.Id == appAction.Receiver_UserRefId).Select(x => x.UserName).FirstOrDefaultAsync();
                var smsTemplateTexts = $"Dear User, Application no. ({application.PublicAppRefNum}) received in your {receiverUerName} account. Kindly Process it - PB LABOUR";
                var templateIds = "1407176009417429959";
                await _iNotificationManagerService.InitiateNotification(appAction.Receiver_UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=smsTemplateTexts, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateIds}
                });
            }
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }


        public async Task<GenericResponseTemplateModel<Application>> GetApplication(Int64 appRefId)
        {
            GenericResponseTemplateModel<Application> genericRespModel = new GenericResponseTemplateModel<Application>();
            try
            {
                genericRespModel.ResponseDataModel = await _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<ApplicationActionViewModel> ADD_PAYMENT_RAISED_FEE(object requestData, Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionViewModel>();
            var viewModel = requestData as ApplicationRaiseFeeParmsViewModel;
            IList<Payments_RaisedFee> listData = new List<Payments_RaisedFee>();
            foreach (var feeInfo in viewModel.FeeCalculatorInfoParms)
            {
                listData.Add(new Payments_RaisedFee
                {
                    AppRefId = appRefId,
                    FeeHeaderRefId = feeInfo.FeeHeaderId,
                    PaymentBatchCounter = feeInfo.PaymentBatchCounter,
                    AmountRaised = feeInfo.AmountCalculated,
                    AmountAlreadyPaid = 0,
                    IsFeeApplicable = true,
                    Createddate = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow,
                    ApplicationActionLogId = 0,
                    NonTreasuryCode = feeInfo.NonTreasuryCode,
                    Description = feeInfo.Description,
                    HasDedicatedTreasuryCode = feeInfo.HasDedicatedTreasuryCode,
                    DedicatedTreasurCode = feeInfo.DedicatedTreasurCode,
                    DedicatedDDOCode = feeInfo.DedicatedDDOCode
                });
            }
            await _context.BulkInsertAsync(listData);

            var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
            string actionName = _context.ApplicationActionCodes.Where(x => x.ActionCode == 401).FirstOrDefault().ActionName;
            genericServiceResultTemplate.ResponseDataModel = new ApplicationActionViewModel()
            {
                AppActionType = 401,
                Receiver_UserRefId = projectSite.UserRefId,
                Receiver_ProfileRefId = _context.UserProfileMapping.Where(x => x.UserRefId == projectSite.UserRefId).Select(x => x.UserProfileRefId).FirstOrDefault(),
                Remarks = actionName,
                AppRefId = appRefId,
                PdfNameGUID = "",
                PublicAppRefNum = "",
                CheckListFormJson = "",
                PaymentBatchCounter = application.PaymentBatchCounter,
                RaisedFeeAmount = listData.Sum(fee => fee.AmountRaised),
                RaisedFeeReason = viewModel.Remarks,
                Workers_MaxDuringYear = 0,
                PowerKW_Installed = 0,
                ExistingWorkers_MaxDuringYear = 0,
                ExistingPowerKW_Installed = 0
            };

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return genericServiceResultTemplate.ResponseDataModel;
        }


        public async Task<bool> INCREASE_PAYMENT_BATCH_COUNTER(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            application.PaymentBatchCounter = application.PaymentBatchCounter + 1;
            _context.Update(application);
            await _context.SaveChangesAsync();
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SET_ACTION_LOG_ID(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var latestActionLogId = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == appRefId).OrderByDescending(x => x.ActionDate)
                .Select(x => x.ApplicationActionLogId).FirstOrDefault();
            int paymentBatchCounter = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Select(x => x.PaymentBatchCounter).FirstOrDefault();
            var payments = _context.Payments_RaisedFee.Where(x => x.AppRefId == appRefId).ToList();

            foreach (var payment in payments)
            {
                payment.PaymentBatchCounter = paymentBatchCounter;
                payment.ApplicationActionLogId = latestActionLogId;
            }
            await _context.BulkUpdateAsync(payments);

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public async Task<bool> UPDATE_PAYMENT_RAISED_FEE(object requestData, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            var viewModel = requestData as ApplicationPaymentDetailViewModal;
            IList<Payments_RaisedFee> listDataToUpdate = new List<Payments_RaisedFee>();
            IList<Payments_RaisedFee_Log> logDataToInsert = new List<Payments_RaisedFee_Log>();

            var paymentRaisedFee = _context.Payments_RaisedFee.Where(x => x.AppRefId == viewModel.AppRefId).FirstOrDefault();

            var applicationActionLogId = paymentRaisedFee.ApplicationActionLogId;

            foreach (var feeInfo in viewModel.ApplicationPaymentDetailList)
            {
                var logEntry = new Payments_RaisedFee_Log
                {
                    AppRefId = viewModel.AppRefId,
                    FeeHeaderRefId = feeInfo.FeeHeaderRefId,
                    PaymentBatchCounter = feeInfo.PaymentBatchCounter,
                    AmountRaised = feeInfo.AmountRaised,
                    AmountAlreadyPaid = feeInfo.AmountAlreadyPaid ?? 0,
                    IsFeeApplicable = feeInfo.IsFeeApplicable,
                    Createddate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    NonTreasuryCode = feeInfo.NonTreasuryCode,
                    ApplicationActionLogId = applicationActionLogId
                };

                logDataToInsert.Add(logEntry);
                var feeToUpdate = _context.Payments_RaisedFee.Where(x => x.AppRefId == viewModel.AppRefId && x.FeeHeaderRefId == feeInfo.FeeHeaderRefId).FirstOrDefault();
                if (feeToUpdate != null)
                {
                    feeToUpdate.AmountRaised = feeInfo.AmountRaised;
                    feeToUpdate.AmountAlreadyPaid = feeInfo.AmountAlreadyPaid;
                    feeToUpdate.LastModifiedDate = DateTime.Now;
                    listDataToUpdate.Add(feeToUpdate);
                }
            }

            await _context.BulkUpdateAsync(listDataToUpdate);
            await _context.BulkInsertAsync(logDataToInsert);

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = viewModel.AppRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public async Task<bool> WITHDRAW_APPLICATION(Int64 appRefId, string remarks, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
            {
                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                new StoreProcedureParm (){ ParmName="Remarks", ParmValue=remarks.ToString(), isNumber=false}
            };

            //await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_WithdrawApplication", storeProcedureParm);
            var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_WithdrawApplication", storeProcedureParm);

            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "Application",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });
            return true;
        }

        public async Task<bool> SEED_TIME_LINE_FLOW_DATA(Int64 appRefId, string rootActivityRefId, ToDoCodeTypeEnum toDoCodeType)
        {
            await _iAppTimeLineManagerService.SeedTimeLineData(appRefId, 0);
            await MapRootActivity(new ToDoTableRootActivityMapping()
            {
                EntityModelName = "ApplicationDirtyStatusMapping",
                PrimaryKeyValue = appRefId,
                RootActivityRefId = rootActivityRefId,
                ToDoCodeType = toDoCodeType
            });

            return true;
        }

        public int GetWorkingDays(DateTime from, DateTime to)
        {
            var totalDays = 0;
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }
            return totalDays;
        }


        public async Task<Int64> GET_ALCCircleId_By_LabourCircleId(Int64 circleRefId)
        {
            var alcCircleId = _context.LabourCircles.Where(x => x.LabourCircleId == circleRefId).FirstOrDefault().ALCCircleRefId;
            return alcCircleId;
        }


    }
}
