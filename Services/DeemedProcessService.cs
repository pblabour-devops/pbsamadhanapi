using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class DeemedProcessService : IDeemedProcessService
    {
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private IPdfOprationsService _IPdfOprations;
        private readonly IApplicationManagementService<ApplicationAction> _iApplicationMamnagementService;
        private readonly INotificationManagerService _iNotificationManagerService;
        private IServiceScopeFactory _iServiceScopeFactory;
        public static List<ApplicationToBeElapsedViewModel> elapsedApplications = new List<ApplicationToBeElapsedViewModel>();
        public static List<ApplicationToBeEscalatedViewModel> escalatedApplications = new List<ApplicationToBeEscalatedViewModel>();
        public IConfiguration Configuration { get; }
        public static List<ApplicationEscalationSlabViewModel> escalationSlabs = new List<ApplicationEscalationSlabViewModel>();
        public static List<EscalationOffcerViewModel> escalationOffcers = new List<EscalationOffcerViewModel>();
        public static List<OfficerUserDetailsViewModel> officerUserDetails = new List<OfficerUserDetailsViewModel>();
        public DeemedProcessService(AppDbContext context, 
            IGeneric_SP_Repository iGeneric_SP_Repository, 
            IPdfOprationsService IPdfOprations,
            IApplicationManagementService<ApplicationAction> iApplicationManagementService,
            INotificationManagerService iNotificationManagerService,
            IServiceScopeFactory iServiceScopeFactory, IConfiguration configuration)
        {
            _IPdfOprations = IPdfOprations;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iApplicationMamnagementService = iApplicationManagementService;
            _iNotificationManagerService = iNotificationManagerService;
            _iServiceScopeFactory = iServiceScopeFactory;
            Configuration = configuration;
            escalationSlabs.Add(new ApplicationEscalationSlabViewModel() { SlabRangeFrom = 0, SlabRangeTo = 49, SlabValue = 49 });
            escalationSlabs.Add(new ApplicationEscalationSlabViewModel() { SlabRangeFrom = 50, SlabRangeTo = 64, SlabValue = 50 });
            escalationSlabs.Add(new ApplicationEscalationSlabViewModel() { SlabRangeFrom = 65, SlabRangeTo = 79, SlabValue = 65 });
            escalationSlabs.Add(new ApplicationEscalationSlabViewModel() { SlabRangeFrom = 80, SlabRangeTo = 100000, SlabValue = 80 });
        }
        public async Task<GenericResponseTemplateModel<bool>> GenerateCertificates(DeemedActionParmsViewModel requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var deemedAbleFiles = await _context.Deemed_ProcessFilesLogs
                    .Where(x => x.DeemedProcessStatusType == DeemedProcessStatusTypeEnum.PENDING && x.ApplicationType == requestData.ApplicationType)
                    .Include(x=>x.Application).ThenInclude(x=>x.ProjectSites)
                    .ToListAsync();

                foreach (var item in deemedAbleFiles)
                {
                    try
                    {
                        var userName = _context.Users.Where(x => x.Id == item.Officer_UserRefId).Select(x => x.UserName).FirstOrDefault();
                        CertificateGenerateServiceResultTemplate certResp = await _IPdfOprations.GenerateCertificate(item.AppRefId, requestData.ApplicationType, userName, true);
                        if (certResp.HasException)
                        {
                            item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.ERROR;
                            item.DeemedProcessRemarks = certResp.Exceptions;
                        }
                        else
                        {
                            ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                            {
                                AppActionType = (int)AppActionTypeEnum.DEEMED_COMPLETED,
                                AppDocumentRefId = 0,
                                AppRefId = item.AppRefId,
                                CheckListFormJson = null,
                                IsDocumentUploaded = false,
                                PdfNameGUID = certResp.PdfNameGUID,

                                PublicAppRefNum = item.Application.PublicAppRefNum,
                                Receiver_ProfileRefId = 0,
                                Receiver_UserRefId = item.Application.ProjectSites.UserRefId,

                                Remarks = "Application approved as deemed..",
                                UserId = "f1ba27b3-27ca-46a6-8bad-609951bf17c4"
                            };
                            var resp = await _iApplicationMamnagementService.RecordApplicationAction(applicationAction, "f1ba27b3-27ca-46a6-8bad-609951bf17c4");
                            if (resp.HasError)
                            {
                                item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.ERROR;
                                item.DeemedProcessRemarks = resp.ErrorDesc;
                            }
                            else
                            {
                                item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.COMPLETED;
                                item.DeemedProcessRemarks = "Deemed process completed";
                                item.CertificateFileName = item.Application.PublicAppRefNum;
                            }
                        }
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="DeemedId", ParmValue= item.DeemedId.ToString(), isNumber=true },
                            new StoreProcedureParm (){ ParmName="DeemedProcessStatusType", ParmValue= ((int)item.DeemedProcessStatusType).ToString(), isNumber=true },
                            new StoreProcedureParm (){ ParmName="DeemedProcessRemarks", ParmValue= item.DeemedProcessRemarks.ToString(), isNumber=false }
                        };
                        var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Deemed_UpdateFileLogs", storeProcedureParms);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        
        public async Task<GenericResponseTemplateModel<bool>> DormantApplications(DormantParmsViewModel requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var dormantFiles = await _context.Dormant_ProcessFilesLogs
                    .Where(x => x.DormantProcessStatusType == DormantProcessStatusTypeEnum.PENDING && x.DormantProcessEngineRefId == requestData.DormantProcessEngineRefId && x.ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                    .Include(x => x.Application).ThenInclude(x => x.ProjectSites)
                    .ToListAsync();

                foreach (var item in dormantFiles)
                {
                    try
                    {
                        var dormantNotification = await _context.Dormant_FileNotifications
                                 .Where(x => x.ApplicationActionLogRefId == item.ApplicationActionLogRefId).FirstOrDefaultAsync();

                        if (dormantNotification == null) // No notification sent yet
                        {
                            Dormant_FileNotification dormant_FileNotifications = new Dormant_FileNotification()
                            {
                                AppRefId = item.AppRefId,
                                ApplicationActionLogRefId = item.ApplicationActionLogRefId,
                                IterationCount = 1,
                                NotificationDate = DateTime.Now
                            };
                            await _context.AddAsync(dormant_FileNotifications);
                            await _context.SaveChangesAsync();

                            // Send Notification
                            var viewModel = new DormantFileNotificationViewModel
                            {
                                ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(item.Application.ApplicationType.ToString()),
                                ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(item.Application.ApplicationPurposeType.ToString()),
                                EstablishmentName = item.Application.ProjectSites.EstablishmentName,
                                PublicApplicationRefNo = (item.Application.InvestPunjab_AppId).ToString(),
                                StatusDescription = "Objection Raised",
                                InvestPunjab_Ipin = "Ipin : " + item.Application.InvestPunjab_Ipin + " / Application No. :" + item.Application.InvestPunjab_AppId,
                                UserRefId = item.Application.ProjectSites.UserRefId,
                                IterationCount = dormant_FileNotifications.IterationCount,
                                DormantInTime = item.DormantInTime
                            };
                            await SendDormantNotificationAsync(viewModel);

                            // Update Dormant Process Status
                            await UpdateDormantFileLogAsync(item.ApplicationActionLogRefId, DormantProcessStatusTypeEnum.NOTIFICATION_SENT);
                        }
                        else
                        {

                            var daysSinceLastNotification = (DateTime.Now - dormantNotification.NotificationDate).TotalDays;

                            bool isRegulatory =
                                item.Application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD ||
                                item.Application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED ||
                                item.Application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING ||
                                item.Application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT;

                            if (
                                ((isRegulatory && daysSinceLastNotification < 7) ||
                                 (!isRegulatory && daysSinceLastNotification < 3)) &&
                                dormantNotification.IterationCount < 3)
                            {
                                dormantNotification.NotificationDate = DateTime.Now;
                                dormantNotification.IterationCount += 1;
                                _context.Update(dormantNotification);
                                await _context.SaveChangesAsync();

                                await UpdateDormantFileLogAsync(item.ApplicationActionLogRefId, DormantProcessStatusTypeEnum.NOTIFICATION_WAITING_TIME);
                            }
                            else if (
                                ((isRegulatory && daysSinceLastNotification >= 7) ||
                                 (!isRegulatory && daysSinceLastNotification >= 3)) &&
                                dormantNotification.IterationCount < 3)
                            {
                                var viewModel = new DormantFileNotificationViewModel
                                {
                                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(item.Application.ApplicationType.ToString()),
                                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(item.Application.ApplicationPurposeType.ToString()),
                                    EstablishmentName = item.Application.ProjectSites.EstablishmentName,
                                    PublicApplicationRefNo = (item.Application.InvestPunjab_AppId).ToString(),
                                    StatusDescription = "Objection Raised",
                                    InvestPunjab_Ipin = "Ipin : " + item.Application.InvestPunjab_Ipin + " / Application No. :" + item.Application.InvestPunjab_AppId,
                                    UserRefId = item.Application.ProjectSites.UserRefId,
                                    IterationCount = dormantNotification.IterationCount == null ? 1 : dormantNotification.IterationCount,
                                    DormantInTime = item.DormantInTime
                                };
                                await SendDormantNotificationAsync(viewModel);

                                dormantNotification.NotificationDate = DateTime.Now;
                                dormantNotification.IterationCount += 1;
                                _context.Update(dormantNotification);
                                await _context.SaveChangesAsync();

                                await UpdateDormantFileLogAsync(item.ApplicationActionLogRefId, DormantProcessStatusTypeEnum.NOTIFICATION_SENT);
                            }
                            else if (dormantNotification.IterationCount == 3)
                            {
                                var userName = _context.Users.Where(x => x.Id == item.Officer_UserRefId).Select(x => x.UserName).FirstOrDefault();

                                ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                                {
                                    AppActionType = (int)AppActionTypeEnum.APPLICATION_DORMANT,
                                    AppDocumentRefId = 0,
                                    AppRefId = item.AppRefId,
                                    CheckListFormJson = null,
                                    IsDocumentUploaded = false,
                                    PdfNameGUID = "NA",
                                    PublicAppRefNum = item.Application.PublicAppRefNum,
                                    Receiver_ProfileRefId = 0,
                                    Receiver_UserRefId = item.Application.ProjectSites.UserRefId,
                                    Remarks = "Application dormant..",
                                    UserId = "C79F57CB-40BA-48EA-8336-327DC1B78701"
                                };

                                var resp = await _iApplicationMamnagementService.RecordApplicationAction(applicationAction, "C79F57CB-40BA-48EA-8336-327DC1B78701");

                                if (resp.HasError)
                                {
                                    item.DormantProcessStatusType = DormantProcessStatusTypeEnum.ERROR;
                                    item.DormantProcessRemarks = resp.ErrorDesc;
                                }
                                else
                                {
                                    item.DormantProcessStatusType = DormantProcessStatusTypeEnum.COMPLETED;
                                    item.DormantProcessRemarks = "Dormant process completed";
                                }

                                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="DormantId", ParmValue= item.DormantId.ToString(), isNumber=true },
                                    new StoreProcedureParm (){ ParmName="DormantProcessStatusType", ParmValue= ((int)item.DormantProcessStatusType).ToString(), isNumber=true },
                                    new StoreProcedureParm (){ ParmName="DormantProcessRemarks", ParmValue= item.DormantProcessRemarks.ToString(), isNumber=false }
                                };
                                var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Dormant_UpdateFileLogs", storeProcedureParms);

                                var viewModel = new DormantFileNotificationViewModel
                                {
                                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(item.Application.ApplicationType.ToString()),
                                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(item.Application.ApplicationPurposeType.ToString()),
                                    EstablishmentName = item.Application.ProjectSites.EstablishmentName,
                                    PublicApplicationRefNo = (item.Application.InvestPunjab_AppId).ToString(),
                                    StatusDescription = "Application Dormant",
                                    InvestPunjab_Ipin = "Ipin : " + item.Application.InvestPunjab_Ipin + " / Application No. :" + item.Application.InvestPunjab_AppId,
                                    UserRefId = item.Application.ProjectSites.UserRefId,
                                    IterationCount = dormantNotification.IterationCount == null ? 1 : dormantNotification.IterationCount,
                                    DormantInTime = item.DormantInTime
                                };
                                await SendDormantMarkedNotificationAsync(viewModel);

                                await UpdateDormantFileLogAsync(item.ApplicationActionLogRefId, DormantProcessStatusTypeEnum.NOTIFICATION_SENT);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        private async Task UpdateDormantFileLogAsync(Int64 actionLogId, DormantProcessStatusTypeEnum statusType)
        {
            var log = await _context.Dormant_ProcessFilesLogs
                .FirstOrDefaultAsync(x => x.ApplicationActionLogRefId == actionLogId);

            if (log == null)
            {
                log = new Dormant_ProcessFilesLog
                {
                    ApplicationActionLogRefId = actionLogId,
                    DormantProcessStatusType = statusType
                };
                await _context.AddAsync(log);
            }
            else
            {
                log.DormantProcessStatusType = statusType;
                _context.Update(log);
            }
            await _context.SaveChangesAsync();
        }


        public async Task SendDormantNotificationAsync(DormantFileNotificationViewModel model)
        {
            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateDormantApplicationNotification", model);

            await _iNotificationManagerService.InitiateNotification(model.UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel()
                    {
                        Body = emailTemplateText,
                        NotificationMode = NotificationModeTypeEnum.EMAIL,
                        NotificationPurpose = NotificationPurposeTypeEnum.INFO,
                        Title = $"Department of Labour: Reminder-{model.IterationCount} Kindly resolve the raised objection associated with File No : ({model.PublicApplicationRefNo})"
                    }
                });

            //var smsTemplateText = $"Reminder-{model.IterationCount} Kindly resolve the raised objection associated with File No : ({model.PublicApplicationRefNo}) -PB LABOUR";
            var smsTemplateText = $"Reminder - {model.IterationCount}, Dear Investor App. no:{model.PublicApplicationRefNo} is pending from {model.DormantInTime} days. Please resolve your objection otherwise it would be dormant -PB LABOUR";
            var templateId = "1407174601324169176";

            await _iNotificationManagerService.InitiateNotification(model.UserRefId,
            new List<NotificationViewModel>()
            {
                new NotificationViewModel()
                {
                    Body = smsTemplateText,
                    NotificationMode = NotificationModeTypeEnum.MOBILE,
                    NotificationPurpose = NotificationPurposeTypeEnum.INFO,
                    Title = templateId
                }
            });
        }

        public async Task SendDormantMarkedNotificationAsync(DormantFileNotificationViewModel model)
        {
            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateDormantMarkedApplicationNotification", model);

            await _iNotificationManagerService.InitiateNotification(model.UserRefId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel()
                    {
                        Body = emailTemplateText,
                        NotificationMode = NotificationModeTypeEnum.EMAIL,
                        NotificationPurpose = NotificationPurposeTypeEnum.INFO,
                        Title = $"Department of Labour: File No : ({model.PublicApplicationRefNo}) Marked as Dormant Due to Unresolved Objection"
                    }
                });

            var smsTemplateText = $"File No : ({model.PublicApplicationRefNo}) Marked as Dormant Due to Unresolved Objection from ({model.DormantInTime}) days -PB LABOUR";
            var templateId = "1407172724884749762";

            await _iNotificationManagerService.InitiateNotification(model.UserRefId,
            new List<NotificationViewModel>()
            {
                new NotificationViewModel()
                {
                    Body = smsTemplateText,
                    NotificationMode = NotificationModeTypeEnum.MOBILE,
                    NotificationPurpose = NotificationPurposeTypeEnum.INFO,
                    Title = templateId
                }
            });
        }


        public async Task<GenericFormModel<bool>> EsclationApplications()
        {
            GenericFormModel<bool> genericFormModel = new GenericFormModel<bool>();
            long newId = 0;
            DateTime escalationDate = DateTime.Now;
            int dormantDays = 0;
            int departmentTakenDays = 0;
            using (var scope = _iServiceScopeFactory.CreateScope())
            {
                try
                {
                    Esclations_ProcessEngine esclations_ProcessEngine = new Esclations_ProcessEngine();
                    esclations_ProcessEngine.TimeStemp = DateTime.Now;
                    await _context.Esclations_ProcessEngine.AddAsync(esclations_ProcessEngine);
                    await _context.SaveChangesAsync();
                    newId = esclations_ProcessEngine.Esclations_ProcessEngineId;
                    escalationDate = esclations_ProcessEngine.TimeStemp;

                    var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                    escalatedApplications = await _iGenericRepo.CallStoreProcedureReaderAsyncWithoutParams<ApplicationToBeEscalatedViewModel>("dbo.sp_GetEscalationPendingApplications");
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();

                }
            }


            try
            {
                foreach (var escalatedApp in escalatedApplications)
                {
                    //ApplicationToBeEscalatedViewModel escalatedApp = escalatedApplications[0];

                    List<AllowedReceiverDetailsViewModel> allowedReceiverDetails = new List<AllowedReceiverDetailsViewModel>();
                    //System.Diagnostics.Debug.WriteLine("Processing AppId : " + escalatedApp.AppRefId.ToString());


                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.FACTORY_LICENCE)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_Factory").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.BUILDING_PLAN_HUD)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_HUD").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.BUILDING_PLAN_EXISTING)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_Existing").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_Addition_Ammendment").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_Proposed").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.CONTRACT_LABOUR)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_CL").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_PE_IM").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_CL_IM").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_BOCW").Value);
                    }

                    if (escalatedApp.ApplicationType == (int)ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                    {
                        escalatedApp.MaxTATDays = Convert.ToInt32(Configuration.GetSection("AppTATSettings").GetSection("MaxTATDays_PE_CL").Value);
                    }


                    // STEP: 1 Calculate Holidays, Weekends and Objection Days
                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= escalatedApp.AppRefId.ToString(), isNumber=true }
                        };
                        var timeTakenActionWise = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DepartmentTimeTakenViewModel>("dbo.sp_Get_TakenTime_ActionWise", storeProcedureParms);

                        long submissionLogId = 0;

                        if (escalatedApp.IsTimeLineFlow == true)
                        {
                            submissionLogId = timeTakenActionWise.Where(x => x.AppActionType == 106).Select(x => x.ApplicationActionLogId).FirstOrDefault();
                        }
                        else
                        {
                            var actionTypes = new[] { 3, 5, 405 }; 
                            submissionLogId = timeTakenActionWise.Where(x => actionTypes.Contains(x.AppActionType)).Select(x => x.ApplicationActionLogId).FirstOrDefault();
                        }

                         departmentTakenDays = timeTakenActionWise.Where(x => x.ApplicationActionLogId > submissionLogId && x.ActionPendingWithType == 2).Sum(x => x.TimeGapInMintue)/1440;
                    }



                    escalatedApp.TotalDaysCount = (DateTime.Now - escalatedApp.SubmissionDate).Days;
                    escalatedApp.NetDaysCount = departmentTakenDays;

                    // STEP: 2 Calculate Percentage Slab
                    escalatedApp.ActualPercentage = (Convert.ToDecimal(escalatedApp.NetDaysCount) / Convert.ToDecimal(escalatedApp.MaxTATDays)) * 100;
                    escalatedApp.PercentageSlab = escalationSlabs.Where(x => x.SlabRangeFrom <= escalatedApp.ActualPercentage && escalatedApp.ActualPercentage <= x.SlabRangeTo).Select(x => x.SlabValue).FirstOrDefault();


                    //using (var scope = _iServiceScopeFactory.CreateScope())
                    //{
                    //    var _iGR_AppEscalations = scope.ServiceProvider.GetRequiredService<AppEscalations>();

                    //    AppEscalations appEscalations =new AppEscalations();
                    //    appEscalations.AppRefId = escalatedApp.AppRefId;
                    //    appEscalations.ApplicationType = escalatedApp.ApplicationType;
                    //    appEscalations.SubmissionDate = escalatedApp.SubmissionDate;
                    //    appEscalations.TotalDaysCount = escalatedApp.TotalDaysCount;
                    //    appEscalations.TotalWorkingDaysCount = escalatedApp.TotalWorkingDaysCount;
                    //    appEscalations.TotalHolidaysCount = escalatedApp.TotalHolidaysCount;
                    //    appEscalations.TotalObjectionDaysCount = escalatedApp.TotalObjectionDaysCount;
                    //    appEscalations.NetDaysCount = escalatedApp.NetDaysCount;
                    //    appEscalations.ActualPercentage = escalatedApp.ActualPercentage;
                    //    appEscalations.PercentageSlab = escalatedApp.PercentageSlab;
                    //    appEscalations.MaxTATDays = escalatedApp.MaxTATDays;
                    //    _iGR_AppEscalations.Insert(appEscalations);
                    //    await _iGR_AppEscalations.SavechangeAsync();
                    //}
                    List<EscalationOffcerViewModel> escalationOffcers = new List<EscalationOffcerViewModel>();
                    using (var scope = _iServiceScopeFactory.CreateScope())
                    {
                        try
                        {
                            //Esclations_ProcessEngine esclations_ProcessEngine = new Esclations_ProcessEngine();
                            //esclations_ProcessEngine.TimeStemp = DateTime.Now;
                            //await _context.Esclations_ProcessEngine.AddAsync(esclations_ProcessEngine);
                            //await _context.SaveChangesAsync();
                            //newId = esclations_ProcessEngine.Esclations_ProcessEngineId;
                            //escalationDate = esclations_ProcessEngine.TimeStemp;

                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                            storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=escalatedApp.AppRefId.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="ApplicationType",ParmValue= escalatedApp.ApplicationType.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="PercentageSlab", ParmValue=escalatedApp.PercentageSlab.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="PendingWithUserId",ParmValue= escalatedApp.PendingWithUserId, isNumber=false},

                            };
                            escalationOffcers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EscalationOffcerViewModel>("dbo.sp_Get_EscalationOfficers", storeProcedureParms);



                        }
                        catch (Exception ex)
                        {
                            string error = ex.ToString();

                        }
                    }

                    if (escalatedApp.PercentageSlab == 50)
                    {
                        int a = 20;

                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            var appescalationid = _context.AppEscalations.Where(x => x.AppRefId == escalatedApp.AppRefId && x.ApplicationType == escalatedApp.ApplicationType && x.PercentageSlab == escalatedApp.PercentageSlab).Select(x => x.Id).FirstOrDefault();

                            if (appescalationid == 0 && appescalationid == null)
                            {
                                AppEscalations appEscalations = new AppEscalations();
                                appEscalations.AppRefId = escalatedApp.AppRefId;
                                appEscalations.ApplicationType = escalatedApp.ApplicationType;
                                appEscalations.SubmissionDate = escalatedApp.SubmissionDate;
                                appEscalations.TotalDaysCount = escalatedApp.TotalDaysCount;
                                appEscalations.TotalWorkingDaysCount = escalatedApp.TotalWorkingDaysCount;
                                appEscalations.TotalHolidaysCount = escalatedApp.TotalHolidaysCount;
                                appEscalations.TotalObjectionDaysCount = escalatedApp.TotalObjectionDaysCount;
                                appEscalations.NetDaysCount = escalatedApp.NetDaysCount;
                                appEscalations.ActualPercentage = escalatedApp.ActualPercentage;
                                appEscalations.PercentageSlab = escalatedApp.PercentageSlab;
                                appEscalations.MaxTATDays = escalatedApp.MaxTATDays;
                                appEscalations.PendingWithUserId = escalatedApp.PendingWithUserId;
                                appEscalations.IsMailNotificationSent = 0;
                                appEscalations.IsSMSNotificationSent = 0;
                                appEscalations.IsMailNotificationSent_Self = 0;
                                appEscalations.IsSMSNotificationSent_Self = 0;
                                appEscalations.FactoryCircleRefId = escalatedApp.FactoryCircleRefId;
                                appEscalations.ALCCircleRefId = escalatedApp.AlcCircleRefId;
                                appEscalations.LabourCircleRefId = escalatedApp.LabourCircleRefId;
                                appEscalations.EscalationProcessEngineRefId = newId;
                                appEscalations.EscalatedUsers = escalationOffcers.FirstOrDefault().Users;
                                await _context.AppEscalations.AddAsync(appEscalations);
                                await _context.SaveChangesAsync();
                            }
                        }
                        System.Diagnostics.Debug.WriteLine("Processing AppId : " + escalatedApp.AppRefId.ToString());
                    }

                    if (escalatedApp.PercentageSlab == 65)
                    {
                        int a = 20;

                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            var appescalationid = _context.AppEscalations.Where(x => x.AppRefId == escalatedApp.AppRefId && x.ApplicationType == escalatedApp.ApplicationType && x.PercentageSlab == escalatedApp.PercentageSlab).Select(x => x.Id).FirstOrDefault();

                            if (appescalationid == 0 && appescalationid == null)
                            {
                                AppEscalations appEscalations = new AppEscalations();
                                appEscalations.AppRefId = escalatedApp.AppRefId;
                                appEscalations.ApplicationType = escalatedApp.ApplicationType;
                                appEscalations.SubmissionDate = escalatedApp.SubmissionDate;
                                appEscalations.TotalDaysCount = escalatedApp.TotalDaysCount;
                                appEscalations.TotalWorkingDaysCount = escalatedApp.TotalWorkingDaysCount;
                                appEscalations.TotalHolidaysCount = escalatedApp.TotalHolidaysCount;
                                appEscalations.TotalObjectionDaysCount = escalatedApp.TotalObjectionDaysCount;
                                appEscalations.NetDaysCount = escalatedApp.NetDaysCount;
                                appEscalations.ActualPercentage = escalatedApp.ActualPercentage;
                                appEscalations.PercentageSlab = escalatedApp.PercentageSlab;
                                appEscalations.MaxTATDays = escalatedApp.MaxTATDays;
                                appEscalations.PendingWithUserId = escalatedApp.PendingWithUserId;
                                appEscalations.IsMailNotificationSent = 0;
                                appEscalations.IsSMSNotificationSent = 0;
                                appEscalations.IsMailNotificationSent_Self = 0;
                                appEscalations.IsSMSNotificationSent_Self = 0;
                                appEscalations.FactoryCircleRefId = escalatedApp.FactoryCircleRefId;
                                appEscalations.ALCCircleRefId = escalatedApp.AlcCircleRefId;
                                appEscalations.LabourCircleRefId = escalatedApp.LabourCircleRefId;
                                appEscalations.EscalationProcessEngineRefId = newId;
                                appEscalations.EscalatedUsers = escalationOffcers.FirstOrDefault().Users;
                                await _context.AppEscalations.AddAsync(appEscalations);
                                await _context.SaveChangesAsync();
                            }


                        }
                        System.Diagnostics.Debug.WriteLine("Processing AppId : " + escalatedApp.AppRefId.ToString());
                    }

                    if (escalatedApp.PercentageSlab == 80)
                    {
                        int a = 20;

                        using (var scope = _iServiceScopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            var appescalationid = _context.AppEscalations.Where(x => x.AppRefId == escalatedApp.AppRefId && x.ApplicationType == escalatedApp.ApplicationType && x.PercentageSlab == escalatedApp.PercentageSlab).Select(x => x.Id).FirstOrDefault();

                            if (appescalationid == null || appescalationid == 0)
                            {
                                AppEscalations appEscalations = new AppEscalations();
                                appEscalations.AppRefId = escalatedApp.AppRefId;
                                appEscalations.ApplicationType = escalatedApp.ApplicationType;
                                appEscalations.SubmissionDate = escalatedApp.SubmissionDate;
                                appEscalations.TotalDaysCount = escalatedApp.TotalDaysCount;
                                appEscalations.TotalWorkingDaysCount = escalatedApp.TotalWorkingDaysCount;
                                appEscalations.TotalHolidaysCount = escalatedApp.TotalHolidaysCount;
                                appEscalations.TotalObjectionDaysCount = escalatedApp.TotalObjectionDaysCount;
                                appEscalations.NetDaysCount = escalatedApp.NetDaysCount;
                                appEscalations.ActualPercentage = escalatedApp.ActualPercentage;
                                appEscalations.PercentageSlab = escalatedApp.PercentageSlab;
                                appEscalations.MaxTATDays = escalatedApp.MaxTATDays;
                                appEscalations.PendingWithUserId = escalatedApp.PendingWithUserId;
                                appEscalations.IsMailNotificationSent = 0;
                                appEscalations.IsSMSNotificationSent = 0;
                                appEscalations.IsMailNotificationSent_Self = 0;
                                appEscalations.IsSMSNotificationSent_Self = 0;
                                appEscalations.FactoryCircleRefId = escalatedApp.FactoryCircleRefId;
                                appEscalations.ALCCircleRefId = escalatedApp.AlcCircleRefId;
                                appEscalations.LabourCircleRefId = escalatedApp.LabourCircleRefId;
                                appEscalations.EscalationProcessEngineRefId = newId;
                                appEscalations.EscalatedUsers = escalationOffcers.FirstOrDefault().Users;
                                await _context.AppEscalations.AddAsync(appEscalations);
                                await _context.SaveChangesAsync();
                            }


                        }

                        System.Diagnostics.Debug.WriteLine("Processing AppId : " + escalatedApp.AppRefId.ToString());
                    }
                }

                var officerUserDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<OfficerUserDetailsViewModel>("dbo.sp_GetAllOfficerUserData");

                foreach (var officer in officerUserDetails)
                {
                        var data = await GetEscalationApplicationWiseData(newId, officer.UserId);
                        if (data.ResponseDataModel.EscalatedApplication.Count() > 0)
                        {
                            EscalationNotificatonDataViewModel escalationNotificatonData = new EscalationNotificatonDataViewModel();

                            var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                            var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                            var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;


                            var escalationparmsPayload = new
                            {
                                escalationProcessEngineRefId = newId,
                                userId = officer.UserId
                            };
                            string escalationparmsPayloadJsonText = JsonConvert.SerializeObject(escalationparmsPayload);

                            var encryptedParms = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(escalationparmsPayloadJsonText, encryptionKey, ivKey);

                            escalationNotificatonData.OfficerUserDetails = officer;
                            escalationNotificatonData.EscalatedApplicationSlabWise = data.ResponseDataModel.EscalatedApplicationSlabWise;
                            escalationNotificatonData.EncryptedParms = encryptedParms;
                            escalationNotificatonData.EscalationDate = escalationDate;
                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateEscalatedApplicationsNotification", escalationNotificatonData);
                            await _iNotificationManagerService.InitiateNotification(officer.UserId,
                            new List<NotificationViewModel>()
                            {
                            new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title ="Department of Labour: Escalation Report"}
                            });

                        
                        }

                }



                //escalatedApplications.RemoveAt(0);

                //if (escalatedApplications.Count() == 0)
                //{
                //    using (var scope = _iServiceScopeFactory.CreateScope())
                //    {
                //        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                //        var escalationdata = _context.AppEscalations.Where(x => x.IsSMSNotificationSent_Self == 0 &&  x.IsMailNotificationSent_Self ==0);

                //    }
                //}
                genericFormModel.FormModel = true;


            }
            catch (Exception ex)
            {

                string error = ex.ToString(); ;
            }


            System.Diagnostics.Debug.WriteLine("Hello ******************");
            return genericFormModel;
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

        public async Task<GenericResponseTemplateModel<EscalationFileAndActWiseDataViewModel>> GetEscalationApplicationWiseData(Int64 escalationProcessEngineRefId, string userId)
        {
            GenericResponseTemplateModel<EscalationFileAndActWiseDataViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<EscalationFileAndActWiseDataViewModel>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="EscalationProcessEngineRefId", ParmValue=escalationProcessEngineRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="UserId",ParmValue= userId, isNumber=false}

                };
                var appWiseData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EscalatedApplicationViewModel>("dbo.sp_Escalation_GetApplicationWiseData", storeProcedureParms);





                List<EscalatedApplicationSlabWiseViewModel> escalatedApplicationSlabs = new List<EscalatedApplicationSlabWiseViewModel>();

                foreach (var item in appWiseData.Select(x => x.ApplicationType).Distinct().ToList())
                {
                    escalatedApplicationSlabs.Add(new EscalatedApplicationSlabWiseViewModel()
                    {
                        ApplicationTypeDesc = appWiseData.Where(x => x.ApplicationType == item).Select(x => x.ServiceName).FirstOrDefault(),
                        MaxDays_TAT = appWiseData.Where(x => x.ApplicationType == item).Select(x => x.MaxDays_TAT).FirstOrDefault(),
                        Slab50 = appWiseData.Where(x => x.ApplicationType == item && x.PercentageSlab == 50).Count(),
                        Slab65 = appWiseData.Where(x => x.ApplicationType == item && x.PercentageSlab == 65).Count(),
                        Slab80 = appWiseData.Where(x => x.ApplicationType == item && x.PercentageSlab == 80).Count(),
                    }); ;
                }

                escalatedApplicationSlabs.Add(new EscalatedApplicationSlabWiseViewModel()
                {
                    ApplicationTypeDesc = "Total",
                    MaxDays_TAT = 0,
                    Slab50 = appWiseData.Where(x => x.PercentageSlab == 50).Count(),
                    Slab65 = appWiseData.Where(x => x.PercentageSlab == 65).Count(),
                    Slab80 = appWiseData.Where(x => x.PercentageSlab == 80).Count()
                });

                genericServiceResultTemplate.ResponseDataModel = new EscalationFileAndActWiseDataViewModel();
                genericServiceResultTemplate.ResponseDataModel.EscalatedApplicationSlabWise = new List<EscalatedApplicationSlabWiseViewModel>();
                genericServiceResultTemplate.ResponseDataModel.EscalatedApplication = new List<EscalatedApplicationViewModel>();
                genericServiceResultTemplate.ResponseDataModel.EscalatedApplicationSlabWise = escalatedApplicationSlabs;
                genericServiceResultTemplate.ResponseDataModel.EscalatedApplication = appWiseData.OrderBy(x => x.ServiceName).ToList();
                
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericFormModel<bool>> DeemedAllActCertificates()
        {
            GenericFormModel<bool> genericFormModel = new GenericFormModel<bool>();
            long newId = 0;
            DateTime escalationDate = DateTime.Now;
            using (var scope = _iServiceScopeFactory.CreateScope())
            {
                try
                {
                    All_Act_Deemed_ProcessEngineLogs engineLogs = new All_Act_Deemed_ProcessEngineLogs();
                    engineLogs.ProcessStartsOn = DateTime.Now;
                    engineLogs.ProcessEndsOn = DateTime.Now;
                    engineLogs.TotalTargetApplications = 0;
                    engineLogs.Description = "NA";
                    await _context.All_Act_Deemed_ProcessEngineLogs.AddAsync(engineLogs);
                    await _context.SaveChangesAsync();

                    newId = engineLogs.AllActDeemedProcessEngineId;
                    escalationDate = engineLogs.ProcessStartsOn;

                    var _iGenericRepo = scope.ServiceProvider.GetRequiredService<IGeneric_SP_Repository>();
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                    storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="AllActDeemedProcessEngineRefId", ParmValue=engineLogs.AllActDeemedProcessEngineId.ToString(), isNumber=true},

                    };
                    await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Deemed_Utility_All_Act", storeProcedureParms);
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();

                }
            }

            try
            {
                DateTime date =  DateTime.Now;

                var deemedAbleFiles = await _context.All_Act_Deemed_ProcessFilesLogs.Where(x => x.DeemedProcessStatusType == DeemedProcessStatusTypeEnum.PENDING && x.DeemedStartsDate.Date <=  date.Date)
                    .Include(x => x.Application)
                        .ThenInclude(x => x.ProjectSites)
                    .ToListAsync();

                //Deemed Intitiate
                List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>();
                storeProcedureParms1 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="DeemedStartsDate", ParmValue=date.ToString("yyyy-MM-dd"), isNumber=false},
                };
                await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Deemed_Application_Intitiate", storeProcedureParms1);
                //End Deemed Intitiate


                foreach (var item in deemedAbleFiles)
                {

                    if(
                        ((item.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE || item.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER ||
                        item.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || item.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR ||
                        item.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR || item.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT ||
                        item.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD || item.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED ||
                        item.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || item.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT ||
                        item.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC) && item.DeemedStartsDate.Date < date.Date)
                        ||
                        (item.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY || item.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP)
                        )
                    {
                        try
                        {

                            var userName = _context.Users.Where(x => x.Id == item.Officer_UserRefId).Select(x => x.UserName).FirstOrDefault();
                            CertificateGenerateServiceResultTemplate certResp = await _IPdfOprations.AllActGenerateCertificate(item.AppRefId, item.ApplicationType, userName, true);
                            if (certResp.HasException)
                            {
                                item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.ERROR;
                                item.DeemedProcessRemarks = certResp.Exceptions;
                            }
                            else
                            {
                                ApplicationActionViewModel applicationAction = new ApplicationActionViewModel()
                                {
                                    //AppActionType = item.IsFeeApplicable == true ? (int)AppActionTypeEnum.DEEMED_WITH_FEE_PENDING : (int)AppActionTypeEnum.DEEMED_COMPLETED,
                                    AppActionType = (int)AppActionTypeEnum.DEEMED_COMPLETED,
                                    AppDocumentRefId = 0,
                                    AppRefId = item.AppRefId,
                                    CheckListFormJson = null,
                                    IsDocumentUploaded = false,
                                    PdfNameGUID = certResp.PdfNameGUID,

                                    PublicAppRefNum = item.Application.PublicAppRefNum,
                                    Receiver_ProfileRefId = 0,
                                    Receiver_UserRefId = item.Application.ProjectSites.UserRefId,

                                    //Remarks = item.IsFeeApplicable == true ? "Application approved as deemed (fee pending)" : "Application approved as deemed",
                                    Remarks = "Application approved as deemed",
                                    UserId = "f1ba27b3-27ca-46a6-8bad-609951bf17c4"
                                };
                                var resp = await _iApplicationMamnagementService.RecordApplicationAction(applicationAction, "f1ba27b3-27ca-46a6-8bad-609951bf17c4");
                                if (resp.HasError)
                                {
                                    item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.ERROR;
                                    item.DeemedProcessRemarks = resp.ErrorDesc;
                                }
                                else
                                {
                                    //item.DeemedProcessStatusType = item.IsFeeApplicable == true ? DeemedProcessStatusTypeEnum.COMPLETED_WITH_FEE_PENDING : DeemedProcessStatusTypeEnum.COMPLETED;
                                    //item.DeemedProcessRemarks = item.IsFeeApplicable == true ? "Deemed process completed With Fee Pending" : "Deemed process completed";
                                    item.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.COMPLETED;
                                    item.DeemedProcessRemarks = "Deemed process completed";
                                    item.CertificateFileName = item.Application.PublicAppRefNum;
                                }
                            }
                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="DeemedId", ParmValue= item.DeemedId.ToString(), isNumber=true },
                                new StoreProcedureParm (){ ParmName="DeemedProcessStatusType", ParmValue= ((int)item.DeemedProcessStatusType).ToString(), isNumber=true },
                                new StoreProcedureParm (){ ParmName="DeemedProcessRemarks", ParmValue= item.DeemedProcessRemarks.ToString(), isNumber=false }
                            };
                                var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_ALL_Act_Deemed_UpdateFileLogs", storeProcedureParms);
                            }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }               
                  
                }

                genericFormModel.FormModel = true;

            }
            catch (Exception ex)
            {

                string error = ex.ToString(); ;
            }

            return genericFormModel;
        }


        public async Task<GenericResponseTemplateModel<List<All_Act_Deemed_ProcessEngineLogsViewModel>>> GetDeemedApplications(DateTime deemedDate, String type)
        {
            GenericResponseTemplateModel<List<All_Act_Deemed_ProcessEngineLogsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<All_Act_Deemed_ProcessEngineLogsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                 {
                     new StoreProcedureParm (){ ParmName="DeemedDate", ParmValue=deemedDate.ToString("yyyy-MM-dd"), isNumber=false},
                     new StoreProcedureParm (){ ParmName="Type", ParmValue=type.ToString(), isNumber=false}
                 };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<All_Act_Deemed_ProcessEngineLogsViewModel>("dbo.sp_GetDeemedApplicationLists", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<DeemedApplicationTimeLineViewModel>>> GetDeemedTimeLineApplications()
        {
            GenericResponseTemplateModel<List<DeemedApplicationTimeLineViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<DeemedApplicationTimeLineViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<DeemedApplicationTimeLineViewModel>("dbo.sp_Get_DeemedTimelineReport");

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

    }
}


