using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace pbsamadhannetcoreapi.Repositories.CustomRepositories
{
    public class ApplicationManagementRepository<T> : IApplicationManagementRepository<T> where T : class
    {
        private readonly IGenericRepository<T> _iGenericRepository;
        private readonly IGenericRepository<Application> _iGR_Application;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly IAuthService _iAuthService;
        private readonly IAppTimeLineManagerService _iAppTimeLineManagerService;
        public ApplicationManagementRepository(AppDbContext context,
            IGenericRepository<T> iGenericRepository,
            IGenericRepository<Application> iGR_Application,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            INotificationManagerService iNotificationManagerService,
            IAuthService iAuthService,
            IAppTimeLineManagerService iAppTimeLineManagerService)
        {
            _context = context;
            _iGenericRepository = iGenericRepository;
            _iGR_Application = iGR_Application;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iNotificationManagerService = iNotificationManagerService;
            _iAuthService = iAuthService;
            _iAppTimeLineManagerService = iAppTimeLineManagerService;
        }
        public async Task<ApplicationInitiateResponseViewModel> InitiateApplication(T entityType, Int64 projectSiteRefId, User user, Int64 iPin, Int64 Legacy_AppFormId, string legacy_NAR, bool legacy_IsMigrated, Int64 investPunjab_AppId, string legacy_LicenceNo, Int64 legacy_AppId, int applicationType, int applicationPurposeType, int projectSiteVersion, bool isFeeApplicable)
         {
            ApplicationInitiateResponseViewModel applicationInitiateResponse;
            //using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ////Calcullate itration count
                    //int alreadyItrationCount = await _context.Applications.CountAsync(x => x.ProjectSiteRefId == projectSiteRefId && x.ApplicationType == application.ApplicationType && x.ApplicationPurposeType == application.ApplicationPurposeType);

                    //application.PublicAppRefNum = "TempPublicRefNumber";
                    //application.ProjectSiteRefId = projectSiteRefId;
                    //application.IterationCount = alreadyItrationCount + 1;
                    //await _context.Applications.AddAsync(application);
                    //await _context.SaveChangesAsync();

                    //application = await _context.Applications.Where(x => x.AppId == application.AppId).FirstOrDefaultAsync();
                    //if (application != null)
                    //{
                    //    string appTypeAbbr = "";
                    //    string appPurposeTypeCode = "";
                    //    if (application.ApplicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                    //    {
                    //        appTypeAbbr = "ER";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH)
                    //    {
                    //        appTypeAbbr = "CL";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                    //    {
                    //        appTypeAbbr = "BP";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                    //    {
                    //        appTypeAbbr = "CM";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                    //    {
                    //        appTypeAbbr = "HUD";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE)
                    //    {
                    //        appTypeAbbr = "SC";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN)
                    //    {
                    //        appTypeAbbr = "BPP";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                    //    {
                    //        appTypeAbbr = "FL";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP)
                    //    {
                    //        appTypeAbbr = "NTS";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                    //    {
                    //        appTypeAbbr = "CL";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                    //    {
                    //        appTypeAbbr = "BPS";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
                    //    {
                    //        appTypeAbbr = "NTF";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                    //    {
                    //        appTypeAbbr = "BOC";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.MIGRANT_WORKMEN_PE)
                    //    {
                    //        appTypeAbbr = "ISM";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
                    //    {
                    //        appTypeAbbr = "MT";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                    //    {
                    //        appTypeAbbr = "PE";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                    //    {
                    //        appTypeAbbr = "CL";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                    //    {
                    //        appTypeAbbr = "BPP";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING)
                    //    {
                    //        appTypeAbbr = "BPS";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                    //    {
                    //        appTypeAbbr = "BPA";
                    //    }
                    //    else if (application.ApplicationType == ApplicationTypeEnum.TRADE_UNION)
                    //    {
                    //        appTypeAbbr = "TU";
                    //    }

                    //    appPurposeTypeCode = application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE ? "G" : application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE ? "R" : "A";

                    //    application.PublicAppRefNum = appTypeAbbr + appPurposeTypeCode + application.IterationCount.ToString() + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + application.AppId.ToString("D8");
                    //    _context.Entry(application).State = EntityState.Modified;
                    //    await _context.SaveChangesAsync();
                    //    //}





                    //    // This variable is used for set ReceiverRoleId static
                    //    string ReceiverRoleId = "6dff5abf-f8d3-4f80-9310-491b1cb2dbe2";
                    //    var SenderRoleId = user.UserRoles.FirstOrDefault().RoleId;

                    //    if (!application.Legacy_IsMigrated)
                    //    {
                    //        //Create action and action logs
                    //        ApplicationAction applicationAction = new ApplicationAction()
                    //        {
                    //            ActionDate = DateTime.Now,
                    //            ActionTakenDaysCount = 0,
                    //            ActionTakenHoursCount = 0,
                    //            AppActionType = 1,
                    //            ApplicationRefId = application.AppId,

                    //            Receiver_ProfileRefId = user.UserProfileMapping.UserProfileRefId,
                    //            Receiver_UserRefId = user.Id,

                    //            Sender_ProfileRefId = user.UserProfileMapping.UserProfileRefId,
                    //            Sender_UserRefId = user.Id,
                    //            Remarks = "Application drafted",
                    //            ReceiverRoleId = ReceiverRoleId,
                    //            SenderRoleId = SenderRoleId,
                    //            AppDocumentRefId = 0,
                    //            IsDocumentUploaded = false,

                    //            IpAddress = "38.183.8.23",
                    //            Latitude = "30.703616",
                    //            Longitude = "76.7623168"
                    //        };

                    //        await _context.ApplicationActions.AddAsync(applicationAction);
                    //        await _context.SaveChangesAsync();

                    //        ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                    //        {
                    //            ActionDate = applicationAction.ActionDate,
                    //            ActionTakenDaysCount = 0,
                    //            ActionTakenHoursCount = 0,
                    //            AppActionType = applicationAction.AppActionType,
                    //            ApplicationRefId = applicationAction.ApplicationRefId,

                    //            Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                    //            Receiver_UserRefId = applicationAction.Receiver_UserRefId,

                    //            Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
                    //            Sender_UserRefId = applicationAction.Sender_UserRefId,
                    //            Remarks = applicationAction.Remarks,
                    //            ReceiverRoleId = applicationAction.ReceiverRoleId,
                    //            SenderRoleId = applicationAction.SenderRoleId,
                    //            AppDocumentRefId = 0,
                    //            IsDocumentUploaded = false,

                    //            IpAddress = applicationAction.IpAddress,
                    //            Latitude = applicationAction.Latitude,
                    //            Longitude = applicationAction.Longitude
                    //        };

                    //        await _context.ApplicationActionLogs.AddAsync(applicationActionlogs);
                    //        await _context.SaveChangesAsync();
                    //    }
                    //}
                    
                    
                    // Call stored procedure here for Initiate-Applications  - Changed On : 02-July-2025
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="ApplicationPurposeType", ParmValue=applicationPurposeType.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=projectSiteRefId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="IsFeeApplicable", ParmValue=isFeeApplicable.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="Legacy_AppId", ParmValue=legacy_AppId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="Legacy_IsMigrated", ParmValue=legacy_IsMigrated.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="Legacy_NAR", ParmValue=legacy_NAR == null ? "" : legacy_NAR.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="Legacy_AppFormId", ParmValue=Legacy_AppFormId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="InvestPunjab_Ipin", ParmValue=iPin.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="InvestPunjab_AppId", ParmValue=investPunjab_AppId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="Legacy_LicenceNo", ParmValue= legacy_LicenceNo==null ? "" : legacy_LicenceNo.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="ProjectSiteVersion", ParmValue=projectSiteVersion.ToString(), isNumber=true},
                    };
                    var result = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationInitiateResponseViewModel>("sp_InitiateApplication", storeProcedureParms);



                    if (result.FirstOrDefault().IsApplicationCreated)
                    {
                        if (!GenericModelOps<T>.TrySetProperty(entityType, "AppRefId", result.FirstOrDefault().AppId))
                        {
                            //transaction.Rollback();
                        }
                        PropertyInfo entityKeyProp = entityType.GetType().GetProperties().FirstOrDefault(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(KeyAttribute)));
                        GenericModelOps<T>.TrySetProperty(entityType, entityKeyProp.Name, 0);
                        _iGenericRepository.Insert(entityType);
                        await _iGenericRepository.SavechangeAsync();

                        applicationInitiateResponse = new ApplicationInitiateResponseViewModel()
                        {
                            AppId = result.FirstOrDefault().AppId,
                            PublicAppRefNum = result.FirstOrDefault().PublicAppRefNum,
                            IsApplicationCreated = true,
                            EntityKeyId = Convert.ToInt64(entityKeyProp.GetValue(entityType)),
                            ErrorDescription=""
                        };
                    }
                    else
                    {
                        applicationInitiateResponse = new ApplicationInitiateResponseViewModel()
                        {
                            AppId = 0,
                            PublicAppRefNum = string.Empty,
                            IsApplicationCreated = false,
                            EntityKeyId = 0,
                            ErrorDescription = result.FirstOrDefault().ErrorDescription
                        };
                    }
                    
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    applicationInitiateResponse = new ApplicationInitiateResponseViewModel()
                    {
                        AppId = 0,
                        PublicAppRefNum = string.Empty,
                        IsApplicationCreated = false,
                        EntityKeyId = 0,
                        ErrorDescription = ex.Message
                    };
                }
            }
            return applicationInitiateResponse;
        }

        public Task<long> RemoveApplication(Int64 appId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAppLastModifiedDate(long AppId)
        {
            try
            {
                Application application = _iGR_Application.GetById(AppId);
                application.LastModifiedOnDate = DateTime.Now;
                _iGR_Application.Update(application);
                await _iGR_Application.SavechangeAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public async Task<string> LockApplication(Int64 AppId, int AppActionType, string remarks)
        {
            try
            {
                var parentObject = await _iGR_Application.GetAsync(x=>x.AppId==AppId, null,x=>x.ProjectSites).ConfigureAwait(false);
                Application application = parentObject.FirstOrDefault();
                application.LastModifiedOnDate = DateTime.Now;
                application.IsLocked = true;
                application.IsAllowEdit = false;
                if(AppActionType == (int) AppActionTypeEnum.APP_SUBMITTED_FEE_NOT_APPLICABLE || AppActionType == (int)AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED || AppActionType == (int)AppActionTypeEnum.APP_SUBMITTED_FEE_NOT_APPLICABLE_APPROVAL_UNDER_PROCESS || AppActionType == (int)AppActionTypeEnum.APP_SUBMITTED_FEE_NOT_APPLICABLE)
                {
                    application.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                else if (AppActionType == (int)AppActionTypeEnum.APP_SAVE_LOCK_FEE_PENDING)
                {
                    application.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                else if (AppActionType == (int)AppActionTypeEnum.ACKNOWLEDGMENR_SUBMITTED_TO_DEPARTMENT_NO_ACTION_REQUIRED)
                {
                    application.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.ACKNOWLEDGMENT_SUBMITTED_COMPLETED;
                }


                application.ApplicationLifeCycleLastStatusOn = DateTime.Now;

                _iGR_Application.Update(application);
                await _iGR_Application.SavechangeAsync();

                // This Methode is used For Update Sender & Receiver After Lock the File
                List<LendingOfficerDetailsViewModel> officers = new List<LendingOfficerDetailsViewModel>();
                try
                {
                    if (AppActionType != 6 && AppActionType != 7)
                    {
                        // For Factory Temporary Licence
                        if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE && application.IsFeeApplicable == false)
                        {
                            AppActionType = 405;
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                        {
                            AppActionType = 209;
                        }
                        else
                        {
                            AppActionType = (application.IsDigitalSignatureRequired && !application.IsDigitalSignatureVerified) ? 7 : (application.IsFeeApplicable ? 2 : 5);
                        }
                    }

                    // For BOVW Auto Approve Licence
                    if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT && AppActionType != 6 && AppActionType != 7)
                    {
                        if(application.IsFeeApplicable)
                        {
                            AppActionType = 2;
                        }
                        else
                        {
                            AppActionType = 12;
                        }
                    }

                    if (AppActionType == 6)
                    {
                        #region Calculate Days & Hours after objection resolve
                        var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == AppId).FirstOrDefaultAsync();
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
                            new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=AppId.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue= AppActionType.ToString(), isNumber=true},

                            new StoreProcedureParm (){ ParmName="TotalWorkingDays", ParmValue= totalWorkingDays.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="TotalWorkingHours", ParmValue= totalWorkingHours.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="Remarks", ParmValue= remarks.ToString(), isNumber=false}
                        };

                        if (parentObject.FirstOrDefault().IsTimeLineFlow)
                        {
                            officers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_TimeLine_SendAppToOfficerAfterObjResolve", storeProcedureParm);
                        }
                        else
                        {
                            officers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_SendAppToOfficerAfterObjResolve", storeProcedureParm);
                        }

                        // Sent Email Notification 
                        var receiverInfo = await _iAuthService.GetUserProfileByUserId(officers.First().ReceiverUserRefId);
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
                        //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: Objection resolved and application re-submitted (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

                        await _iNotificationManagerService.InitiateNotification(officers.First().ReceiverUserRefId,
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

                        if(application.ApplicationType != ApplicationTypeEnum.MOTOR_TRANSPORT && application.IsTimeLineFlow)
                        {
                            await _iAppTimeLineManagerService.SeedTimeLineData(AppId,0);
                        }

                        // Sent SMS Notification To Officer
                        var receiverUerName = await _context.Users.Where(x => x.Id == officers.First().ReceiverUserRefId).Select(x => x.UserName).FirstOrDefaultAsync();
                        var smsTemplateTexts = $"Dear User, Application no. ({application.PublicAppRefNum}) received in your {receiverUerName} account. Kindly Process it - PB LABOUR";
                        var templateIds = "1407176009417429959";
                        await _iNotificationManagerService.InitiateNotification(officers.First().ReceiverUserRefId,
                        new List<NotificationViewModel>()
                        {
                            new NotificationViewModel(){Body=smsTemplateTexts, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateIds}
                        });
                    }
                    else if (AppActionType == 7) // Balance Fee Recovery
                    {
                        return "NAN";
                    }
                    else
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            //new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=((int)application.ApplicationType).ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=AppId.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="AppActionType", ParmValue= AppActionType.ToString(), isNumber=true}
                        };

                        officers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("dbo.sp_SendApplicationToLendingOfficers", storeProcedureParms);

                        if (application.ApplicationType != ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                        {
                            // Sent Email Notification
                            var receiverInfo = await _iAuthService.GetUserProfileByUserId(officers.First().ReceiverUserRefId);
                            TemplateNewCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateNewCaseLendedToOfficerNotificationViewModel()
                            {
                                OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                                ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
                                ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
                                EstablishmentName = application.ProjectSites.EstablishmentName,
                                PublicApplicationRefNo = application.PublicAppRefNum
                            };
                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateNewCaseLendedToOfficerNotification", mailTemplate);
                            //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: New application received (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

                            await _iNotificationManagerService.InitiateNotification(officers.First().ReceiverUserRefId,
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
                            var receiverUerName = await _context.Users.Where(x => x.Id == officers.First().ReceiverUserRefId).Select(x => x.UserName).FirstOrDefaultAsync();
                            var smsTemplateTexts = $"Dear User, Application no. ({application.PublicAppRefNum}) received in your {receiverUerName} account. Kindly Process it - PB LABOUR";
                            var templateIds = "1407176009417429959";
                            await _iNotificationManagerService.InitiateNotification(officers.First().ReceiverUserRefId,
                            new List<NotificationViewModel>()
                            {
                                new NotificationViewModel(){Body=smsTemplateTexts, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateIds}
                            });
                        }

                       // await _iAppTimeLineManagerService.SeedTimeLineData(AppId);
                    }
                }
                catch (Exception ex)
                {
                    return "";
                    throw ex;
                }
                return officers.First().ReceiverUserRefId;
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
        }

        public async Task<bool> SwitchApplicationLifeCycleType(long AppId, ApplicationLifeCycleStatusTypeEnum appLifeCycleStatusType)
        {
            try
            {
                Application application = await _context.Applications.Where(x => x.AppId == AppId && x.IsDeleted == false).FirstOrDefaultAsync();

                application.ApplicationLifeCycleLastStatusOn = DateTime.Now;
                application.ApplicationLifeCycleStatusType = appLifeCycleStatusType;
                application.IsAllowEdit = true;
                application.IsLocked = false;
                if (appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED 
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.REJECTED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.DEEMED_INITIALIZED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_INITIALIZED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.ACKNOWLEDGMENT_SUBMITTED_COMPLETED
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING
                    || appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT)
                {
                    application.IsAllowEdit = false;
                    application.IsLocked = true;
                }
                else if(appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION)
                {
                    application.IsAllowEdit = true;
                    application.IsLocked = false;
                }
                if(appLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE)
                {
                    application.IsAllowEdit = false;
                    application.IsLocked = true;
                    application.IsFeeApplicable = true;
                }
                application.LastModifiedOnDate = DateTime.Now;
                _context.Entry(application).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception Ex)
            {
                return false;
            }
        }

        public async Task<GenerateLicenceNoViewModel> GenerateLicenceNo(Int64 appId, ApplicationTypeEnum applicationType)
        {
            GenerateLicenceNoViewModel generateLicenceNo = new GenerateLicenceNoViewModel(); 
            {
                try
                {
                    var application = await _context.Applications.Where(x => x.AppId == appId && x.IsDeleted == false).FirstOrDefaultAsync();
                    var projectSite = await _context.ProjectSites.Where(x=> x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefaultAsync();
                    string districtAliasName = await _context.Districts.Where(x=> x.DistrictLgdId == projectSite.DistrictRefId).Select(x=> x.DistrictAliasName).FirstOrDefaultAsync();
                    if (application != null)
                    {
                        string appType = "";
                        if (application.ApplicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                        {
                            appType = "01";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH)
                        {
                            appType = "02";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                        {
                            appType = "03";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                        {
                            appType = "04";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                        {
                            appType = "05";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE)
                        {
                            appType = "06";
                        }

                        var licenceNo = districtAliasName + '/' + 'N' + appType + '/' + projectSite.ProjectSiteId;

                        ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping ()
                        {
                            AppRefId = appId,
                            LicenceNumber = licenceNo
                        };

                        await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
                        await _context.SaveChangesAsync();

                        generateLicenceNo.LicenceNo = licenceNo;
                    }
                }
                catch (Exception ex)
                {
                    generateLicenceNo = new GenerateLicenceNoViewModel()
                    {
                        LicenceNo = string.Empty
                    };
                }
            }
            return generateLicenceNo;
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
    }
}
